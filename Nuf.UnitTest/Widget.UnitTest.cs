using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

using static System.Console;

namespace Nuf.UnitTest
{
    public class Widget
    {
        //[Fact]
        private void GetTestData_should_()
        {
            // Arrange
            var dal = new DataLayerSimulator();

            // Act
            var docs = dal.GetTestData();

            // pretty print :: dbg
            docs.ToList().ForEach(x =>
            {
                WriteLine($@"{x.DocumentId}");
            });

            // Assert
        }

        //[Fact]
        private void Documents_should_()
        {
            // Arrange
            var dal = new DataLayerSimulator();

            // Act
            var docs = dal.Documents;

            // pretty print :: dbg

            docs.ToList().ForEach(x =>
            {
                WriteLine($@"{x.DocumentId}");
            });

            // Assert
            //Assert.True(1 == 1);
        }

        //[Fact]
        private void DocumentById_should_()
        {
            // Arrange
            var dal = new DataLayerSimulator();

            // Act
            var docs = dal.Documents;

            // pretty print :: dbg

            docs.ToList().ForEach(x =>
           {
               var y = dal.DocumentById(x.DocumentId.Value);
               WriteLine($@"{x.DocumentId} : {y.DocumentId} ");
           });

            // Assert
            //Assert.True(1 == 1);
        }

        //[Fact]
        private void NaiveLogic_should_()
        {
            // Arrange
            var logic = new NaiveLogic();

            // Act

            var documentViewModels = logic.GetDocumentsViewModel();// 7.515sec

            documentViewModels.Documents.ForEach(document => {
                var documentViewModel = logic.GetDocumentViewModel(document.DocumentId.Value);
            });

            // pretty print :: dbg
            // Assert
        }

        //[Fact]
        private void BetterLogic_should_()
        {
            // Arrange
            var logic = new BetterLogic();

            // Act
            var documentViewModels = logic.GetDocumentsViewModel();//1.111sec

            documentViewModels.Documents.ForEach(document =>{
                var documentViewModel = logic.GetDocumentViewModel(document.DocumentId.Value);
            });


            //var doc = logic.GetDocumentViewModel(docs.Documents[0].DocumentId.Value);

            // pretty print :: dbg
            // Assert
        }
    }

    public class DataLayerSimulator
    {
        private bool TimeDelay = true;

        public List<Document> GetTestData()
        {
            // Doc A.0 > Doc A.1 > Doc A.2
            // Doc B.0 > Doc B.1 > Doc B.2
            // Doc C.0 > Doc C.1
            // Doc D.0

            var sampleData = new List<Document>
            {
                createDocument("00000000-0000-0000-0000-000000000000", "DocA0", "2018/01/01", true),
                createDocument("B0000000-0000-0000-0000-000000000000", "DocB0", "2018/01/02", true),
                createDocument("C0000000-0000-0000-0000-000000000000", "DocC0", "2018/01/03", true),
                createDocument("D0000000-0000-0000-0000-000000000000", "DocD0", "2018/01/10", false),

                createDocument("A1000000-0000-0000-0000-000000000000", "DocA1", "2018/01/10", true, "A0000000-0000-0000-0000-000000000000"),

                createDocument("B1000000-0000-0000-0000-000000000000", "DocB1", "2018/01/10", true, "B0000000-0000-0000-0000-000000000000"),
                createDocument("C1000000-0000-0000-0000-000000000000", "DocC1", "2018/01/10", false, "C0000000-0000-0000-0000-000000000000"),
                createDocument("A2000000-0000-0000-0000-000000000000", "DocA2", "2018/01/20", false, "A1000000-0000-0000-0000-000000000000"),

                createDocument("B2000000-0000-0000-0000-000000000000", "DocB2", "2018/01/20", false, "B1000000-0000-0000-0000-000000000000")
            };

            return sampleData;
        }

        public IQueryable<Document> Documents
        {
            get
            {
                if (TimeDelay) Thread.Sleep(1000); // Pretend time penalty every time we query the database

                return GetTestData().AsQueryable();
            }
        }

        public Document DocumentById(Guid id)
        {

            if (TimeDelay) Thread.Sleep(500); // Pretend time penalty every time we query the database.  Not so severe when getting a single record.

            return GetTestData().FirstOrDefault(x => x.DocumentId == id);
        }

        public static Document createDocument(string id, string name, string createdon, bool archived, string parentid = null)
        {
            return new Document
            {
                DocumentId = Guid.Parse(id),
                Name = name,
                CreatedOn = DateTime.Parse(createdon),
                Archived = archived,
                PreviousDocumentId = (parentid != null) ? Guid.Parse(parentid) : (Guid?)null
            };
            //if (parentid != null) result.PreviousDocumentId = Guid.Parse(parentid);
            //return result;
        }
    }

    // Main DB entity - reflects the table structure
    public class Document
    {
        public Guid? DocumentId { get; set; }
        public Guid? PreviousDocumentId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Archived { get; set; }

        /// <summary>
        /// Lazy loaded parent property - runs another db query when called.  WARNING: Suggest you avoid using this if poss as it carries a perf penalty.
        /// </summary>
        public Document Parent
        {
            get
            {
                if (PreviousDocumentId == null)
                {
                    return null;
                }
                else
                {

                    return new DataLayerSimulator().DocumentById(PreviousDocumentId.GetValueOrDefault());
                }
            }
        }
    }

    // A single document, suitable for use on a detail page for example.
    public class DocumentViewModel
    {
        public Guid? DocumentId { get; set; }

        // Note: I've changed the problem statement slightly - days since created turns out to be trivially derived, but the below still captures the essence of the problem I think (although you could denormalise to fix!)
        public int DaysSinceFirstVersionWasCreated { get; set; }

        public int DaysSincePreviousVersionWasCreated { get; set; } // An easier/slightly different problem - get parent can be done in two queries but multi level items can't.
    }

    // A collection of documents, suitable for use on a list page for example
    public class DocumentsViewModel
    {
        public DocumentsViewModel()
        {
            Documents = new List<DocumentViewModel>();
        }

        public List<DocumentViewModel> Documents { get; set; }
    }

    public interface ILogic
    {
        DateTime DateToday { get; }

        // Get the specified document
        DocumentViewModel GetDocumentViewModel(Guid documentId);

        // Get all the current documents
        DocumentsViewModel GetDocumentsViewModel();
    }

    // The logic - here is where we need to solve the problem
    //public class Logic : ILogic
    //{
    //    // Use this when calculating date differences so that test code is simpler
    //    public DateTime DateToday => new DateTime(2018, 01, 20);
    //
    //    // Get the specified document
    //    public DocumentViewModel GetDocumentViewModel(Guid documentId)
    //    {
    //        throw new NotImplementedException();
    //    }
    //
    //    // Get all the current documents
    //    public DocumentViewModel GetDocumentsViewModel()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public  class NaiveLogic : ILogic
    {
        // Use this when calculating date differences so that test code is simpler
        public DateTime DateToday => new DateTime(2018, 01, 20);

        private DataLayerSimulator Data => new DataLayerSimulator();

        public DocumentViewModel GetDocumentViewModel(Guid documentId)
        {
            // dal touch
            var d = Data.DocumentById(documentId);

            Document earliest = d;
            Document firstParent = d;

            while (earliest.Parent != null)
            {
                // dal touch
                earliest = earliest.Parent; // Yikes, multiple queries per document!

                if (firstParent.DocumentId == d.DocumentId) firstParent = earliest; // Only once
            }

            return new DocumentViewModel
            {
                DocumentId = d.DocumentId,
                DaysSinceFirstVersionWasCreated = (DateToday - earliest.CreatedOn).Days,
                DaysSincePreviousVersionWasCreated = (DateToday - firstParent.CreatedOn).Days, // Repeated business logic not good
            };
        }

        // Get all the current documents
        public DocumentsViewModel GetDocumentsViewModel() // 7000ms
        {
            // 1000ms
            var documentIds =
                Data.Documents
                .Where(x => x.Archived == false).Select(x =>new { x.DocumentId });

            var documentsViewModel = new DocumentsViewModel();
            foreach (var documentId in documentIds)
            {
                // 6,516ms
                documentsViewModel
                    .Documents
                    .Add(
                            GetDocumentViewModel(documentId.DocumentId.GetValueOrDefault())
                    ); // Will run at least a couple of queries on every non-archived doc in the system!
            }

            return documentsViewModel;
        }
    }

    #region

    public static class BetterLogicDocHelper
    {
        // Use this when calculating date differences so that test code is simpler
        private static DateTime DateToday => new DateTime(2018, 01, 20);

        public static int DaysSinceCreated(this Document earliestVersion)
        {
            if (earliestVersion == null)
            {
                return 0;
            }
            return (DateToday - earliestVersion.CreatedOn).Days;
        }

        public static Document Earliest(this Document d, IQueryable<Document> documents)
        {
            Document earliest = d;

            while (earliest != null && earliest.PreviousDocumentId != null)
            {
                earliest = earliest.Parent(documents);
            }

            return earliest;
        }

        public static Document Parent(this Document d, IQueryable<Document> documents)
        {
            if (d.PreviousDocumentId != null)
            {
                return documents.FirstOrDefault(x => x.DocumentId == d.PreviousDocumentId);
            }
            return null;
        }
    }

    internal class BetterLogic : ILogic
    {
        // Single place to instantiate documents
        private DocumentViewModel createDocumentViewModel(Document doc, Document firstVersion)
        {
            return new DocumentViewModel
            {
                DocumentId = doc.DocumentId,
                DaysSinceFirstVersionWasCreated = firstVersion.DaysSinceCreated()
            };
        }

        // Use this when calculating date differences so that test code is simpler
        public DateTime DateToday => new DateTime(2018, 01, 20);

        private DataLayerSimulator Data => new DataLayerSimulator();

        private DocumentViewModel GetDocumentViewModel(Guid documentId, IQueryable<Document> documents)
        {
            var document = documents.FirstOrDefault(x => x.DocumentId == documentId);

            var earliest = document.Earliest(documents);

            return createDocumentViewModel(document, earliest);
        }

        public DocumentViewModel GetDocumentViewModel(Guid documentId)
        {
            var ds = Data.Documents; // We could have cached this at class level, but I'm choosing to refresh every time I hit the method

            return GetDocumentViewModel(documentId, ds);
        }

        // Get all the current documents :: 1.062ms
        public DocumentsViewModel GetDocumentsViewModel()
        {
            // prefetch all documents
            var documents = Data.Documents;//1sec

            var documentIds = 
                documents
                .Where(x => x.Archived == false).Select(x => new { x.DocumentId });

            // create doc-view model given docs already got ...
            var documentsViewModel = new DocumentsViewModel();

            foreach (var id in documentIds)// 78ms
            {
                var documentId  = id.DocumentId.GetValueOrDefault();

                // get document view model using pre-fetched documents
                var documentViewModel = GetDocumentViewModel(documentId, documents);

                documentsViewModel.Documents.Add(documentViewModel);
            }

            return documentsViewModel;
        }
    }

    #endregion
}