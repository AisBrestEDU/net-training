# Async I/O

Materials for self-education:

###MSDN:
1. [Asynchronous Programming with Async and Await](http://msdn.com/async)
2. [The Task-based Asynchronous Pattern, Stephen Toub, 2012](http://www.microsoft.com/en-us/download/confirmation.aspx?id=19957)
3. [Async Performance: Understanding the Costs of Async and Await](http://msdn.microsoft.com/en-us/magazine/hh456402.aspx)
4. [Asynchronous Programming Patterns](http://msdn.microsoft.com/en-us/library/jj152938.aspx)
5. [How to: Configure Network Tracing](http://msdn.microsoft.com/en-us/library/ty48b824.aspx)

###Test task:
Please implement AsyncIO\Tests.cs

It is easy to solve this task using NET 4.5 and new async-await feature.

So, if you want to switch to NET 4.5 please make sure you have it installed, open project properties and select **Target Framework** as **.NET Framework 4.5** for both projects.
In this case please add **async** keyword to any task method you found it reasonable.

It is also possible to solve these tasks using NET 4.0 but it is a bit more complex and sophisticated.


###NOTE:
1. Use can see the debug information during test if оpen Output window select output from "Debug". Hope it helps you understand the request-response flow. It works in Debug mode only.
2. Please check the performance of **GetUrlContent** against **GetUrlContentAsync** 
