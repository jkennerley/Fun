

Ch1

Fun :
  -  emphasizes fun that apllied to data
  - avoiding state mutation

Funs are first class : 
Avoid state mutation :  see classic example of => is.Sum and => Sort(); is.Sum() on 2 threads that cause false result

OOP : encaps, data abstraction, inheritence, polymorphism
  - Encapsulation
  - Data Abstraction
  - Inheritence
  - Polymorphism : same name, different functionality ; 
     - override, run-time poly ()
	 - overloading, compile-time polymorphism (ad-hoc polymorphism)
	 - generics (parametric polymorphorsm)
 - 

C# Functional ?
 - LINQ
 - Expression bodied memebers
 - C#6/C#7
   - using static
   - getter only property, that can be set in thee ctor
   - local fun variables
   - tuple syntax

Futures : 
  - record types, boiler plate free immutable
  - algebraic types, 
  - pattern matching PARTIAL DONE
  - better syntax for tuples DONE

Functions,  Maths
  a map takes a valaue from an item in DOMAAIN to an item in 
  a CODOMAIN.
  char -> char

  C# Functions
   - Methods
   - Delegates : typesafe fun ptrs ; there is delegate type and then declare instance
   - Lambdas
   - Dictionary (Map, HashTable)

Func, Action delegates
  Func<R> : returns an r
  Func<T1,R> : takes t1 and rets r
So, you can use Func<> instead of delegate
Action ns have args but return void, So
  Action , Action<T1> , Action<T1,T2>

Note,  Predicate<T> (.NET2), and used in FindAll(), 
but in NET3, the where method takes Func<T,bool>.

Fun Arity, the number of args ; nullary, unsary, binary, ternery

So, Func<>, Action<>, and Predicate<> conveys meaning better.

Dictionary
  - var frenchFor = new Dictionary<int,string>{ [true] = "Vrai" , [false] = "Faux" }

  Higher Order Functions HOF
    HOF  : a fun that takes a fun as an args or return a function 
	IterativeApply(f,...) { for(...) f(,,,)  }
	ConditionalApply(f,...) { if(...) f(,,,)  }
	IE<T> Where<T>( IE<T> xs , predicate){  foreach( x in xs){  if(predicate(x)  yield x} }

class Cache<T>
{
  public T Get(id) => ...
  public T Get( id, Func onMiss ) => Get(id) ?? onMiss(id) ;
}


HOF, Callback, Continuation ...
 - the caller of the HOF decides what to do ...
 - the callee decides when to do it ...

HOF : Adapter functions ...
  - adapt the args of functions to something else ...
  static Func<> SwapArgs( this Func<> f ) => (t2,t1) => f(t1,t2) ;
  var divBy_ = divide.SwapArgs();
  divBy(2,10)  // =>5
  See OOP adapter pattern...

HOF : inversion of control ...

HOF : Functions,  that return other functions
  - a fun that returns mod-N
  Func<int,bool> isMod(int n) =>  i => % n == 0   ;
  Func<int,bool> isMod(int n) => (i => i % n == 0);
  Range(1,20)

HOF : avoid duplication ; encapsulating setup/teardown ;  Hole-in-the-middle
var  cns= "" ; 
using( var cn =  new SqlConnection(cns))
{
	cn.Open();
	var xs = cn.Query<Person>(sql)	;
}
return xs;


+ public
_ private
^ protected
> internal

So, wrap up the using and th tear-down ...
  + static RunQuery<T> ( cns , sql ) {
    using( var cn =  new SqlConnection(cns)){
		cn.Open();
		return f(cn)
	}
 }
 
 var xs = Connect<Person>(cns , "select * from Person" ) ; 



 Benefits of FP
  - HOFS, avoid duplication, achieve better sepration of concern
  - conciseness, achieve the same with fewer lines of code, because plumbing is moved behind the scene, surfacing the intent
  - cleaner code more testable, more readable, better support for concurrency, 
  

  OOP : 
  FP : emphasizes funs applied to data, mappings, rather than state mutation ; ...
       FP uses HOFS, hence funs going in/out of other funs ;  may cause side-effect





Why Function Purity Matters
 - Pure funs have some very desirable properties
 - Pure functions ease testability
 - pure functions help correctness


 Pure Functions
  - Math function only calculate a value
  - Program funcs may 1. have side-effect 2. calculate something or both
  - A fun whose return value is affected by more thsn what you send in, is not pure 
      e.g. sideEffectAdd(1,1) // may throw you back 2, or it may throw in an extra thousand -> 1002
	and it is more tricky to test, 
  - a function that returns void, probably has side effect, 
  - a function that returns and side-effects probbaly has 2 concerns ...!
  -   PureFun  : output depends entirely on input ; cause no side-effect
  - ImpiureFun : Factors other than input 

  Side-Effect Fun
    - mutates global state
	- mutates its input args : mutatings args, means the fun is coupled to the caller
	- performs any I/O 
	- ** throws excptions ** 

	Throwing an exception means 
	  1.  The program crashes, BIG side-effect 
	  2.  exception handling up the different call stack may cause indeterminate differnt behaviours
	  

Pure Functions,  
  - easy to test and reason about
  - order is not important




  Managing Side-Effects
   - Isolate I/O 
   - Avoid Mutating args: you can enforce this by only using immutables
   - Handle errors without throwing exceptions
   - avoid mutation

2.2 Purity and Concurrency
2.2.1 pure functions parrallelise well 

   Asynchrony   : program performs non-blocking operations
   Multithreads : software threads (one person talks with 1+ people at the same time)
   Parrallel    : leverages hardware of multi-core machines execs tasks at the same time, 2 things at the ssame time

   pure functions can be pard, non-pure functions may not par.

2.2.3 Avoid State mutation.
  
  The case for Static Methods ...
    'The overuse of static methods'
	  - when act on mutable static fields
	  - perform I/O
    The above 2 reasons mean the static fun is impure

2.3 Pure and Testability
  
2.3.1 In Practice : a validation scenario
  make money transfer 
    Start 
	-> TransferRequest 
	-> ValidRequest
	 ->   BookTransfer
	 ->   End
	-else
	 ->   End
	   
2.3.2 Bringing Impure Functions under Test

  



Cha2 Summary : 
   - compared to math funs, programming functions are more difficult becuase they either depend on or create side-effect
   - side-effect : exception ; state mutation ; throwing exceptions ; 
   - functions withtou side effect are called pure ; 
   - pure funs more easily testable ; can be par'd
   - I/O cannot be avoided, so to reduce foorprint, isolate it ...
   

3. Designing Function Signatures and Types 
 - 




 TODO :  why null is bad abd Option<T>is better


 
 3.4.2
 
 3.4.3 Implementing Option

 Option Concept : 
   - None indoecates abscence of value ; 
   - Some wraps that value ; 
   - conditionally execute code with Match

 3.4.4
   use None and Match instead of Null check its betters since

e.g.  a string field that was required becomes not required ;  put in Option<string> forename ; compilation breaks and you id all the isses and fix them rather than at runtime 

3.4.5 Option returned from partial functions ...
  Total Functions defined for every element of the domain
  Partial : defined for some elements of the domain ; eg, parsing a string->int; return an Option when it could not be parsed
  Opt
  ion<int> Parse(s) => int result ; return int.TryParse(s, out result) ? Some(result) : None ;



  GUARD AGAINST NULL REFERENCE  : 
    NEVER WRITE A FUNCTION THAT RETURNS NULL;
	always check inpus to public methods 
	-


	Usr Option<> in function signatures to achieve honesty

	Summary
	 - make fun signatures as specific as possible
	 - make funs honest ; they always do the what the signature says
	 - use custome types rather than validation to constrain ; use smart constructors
	 - Option is possible abscence of a value
	 - an option is None or Some 
	 - use Match to evaluate None and Some cases


	 4 Patterns on Functional Programming
	   - Map, Bind, Where, ForEach
	   - Functors, Monads
	   - working at different levels of abstraction


4.1. Select is more commonly Known as Map 
  

4.1.3
  make pies, from bag, that may or may not contain pies


4.1.4
  Map appies a function to the inner value.
  The set culd be a IE, Dictionary, Tree, ...
  Map : ( C<T> , (T->R) ) -> C<R>
   A Type that carries a Map is a **FUNCTOR**
   The map fun should be a pure function, with no side effects

4.2 ForEach, Side effect ...


4.3 Chaining Functions With Bind

4.3.1 Combing Option retuirning functions
  Option.Bind option<T> -> T->Option<R> -> Option<R> 

  e.g. in the case of a string age to an Age
    string -> int -> age
	but each stage may fail...

          :: Some(int)    :: Some(Age)  
	"1"   -> 1            -> Age(1)
	"X"   -> None         -> None
	"160" -> 160          -> None

4.3.2 -  flatten nested lists with Bind()... (it liske LINQ.SelectMany)
4.3.3 - actually all this si called Monad ...
  Bind :  ( C<T> , T-> C<R>  ) -> C<R>

  So, Functor is a class that has Map
  Monad is a class that has Bind (**monadic bind **)

4.3. The Return function
  Monads must have a Return function, 
  that lifts a T into a C<T>.


5.2.2 Writing Fun that compose Well
  - Pure
  - Chainable 
  - General
  - Shape-Preserving

  Actions are dead-ends, so not chainable




5.3 Programming Workflows
  - a meaningful seq.  of operations leading to a result 
  - wfs can be modelled thru fun composition, each operation in the workflow can be performed by a function
  - each operation can be performed by a function 
  - those functions can be composed into pipelines
  - 
 5.3.1 Simple workflow for validation
  - e.g 
     - Validate the transfer
	 - Book a transfer
	First doi twith an if

5.3.2 MakeTransfer, but with extensions methods on option
  Some(rq) // lift the transfer into an option
   .Where( isValid) // send the option into the isValid function
   .ForEach(  doit ) // make the apple pie, is the side effect

   define a set of functions
   , each step is a function in the workflow
   , the composition  is the workflow


5.4 Functional Domain Modelling
  
  See Account OOP style and FP style ...


  5.5.3 Take on Layering ...
  rigis and more prone to impurity ....
   ->
      -> 
	    ->
		<-
	<-
  <-

  OR 
   - more fliexible and  mid level cpts can be kept pure

- Controller
-- Validator
---- Repo
--- Account
----- Repo
------ Swift










  