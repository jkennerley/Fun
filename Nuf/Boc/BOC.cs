using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;


namespace Nuf.Boc
{
    public abstract class Command
    {
    }

    public sealed class MakeTransfer : Command
    {

        public Guid DebitedAccountId { get; set; }

        public string Beneficiary { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }

    public interface IValidator<T>
    {
        bool IsValid(T t);
    }

    public sealed class BicFormatValidator : IValidator<MakeTransfer>
    {
        private static readonly Regex regex = new Regex("^[A-Z]{6}[A-Z1-9]{5}$");

        public bool IsValid(MakeTransfer cmd) =>
            regex.IsMatch(cmd.Iban);
    }

    public sealed class DateNotPastValidator : IValidator<MakeTransfer>
    {
        //private readonly IDateTimeService clock;
        private readonly DateTime today ;

        public DateNotPastValidator(DateTime today )
        {
            //this.clock = clock;
            this.today = today ;
        }


        public bool IsValid(MakeTransfer cmd) => cmd.Date >= DateTime.UtcNow.Date;

    }

    public sealed class BicExistsValidator : IValidator<MakeTransfer>
    {
        private Func<IEnumerable<string>> getValidCodes;

        public BicExistsValidator (Func<IEnumerable<string>> getValidCodes_)
        {
            this.getValidCodes = getValidCodes_;
        }


        public bool IsValid(MakeTransfer cmd) => getValidCodes().Contains(cmd.Bic);

    }


    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }


    public class DefaultDateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }














}