# InProcessMessaging
InProcessMessaging defines a lightweight infrastructural component that allows different components within a system to interact in a messaging (pub/sub) fashion as opposed to using dependency injection and resolution.  
  1. This promotes loose coupling between various components wich allows for asynchronously communication within a process space.  
  2. This can also be extended across process spaces using forwarding mechanisms. 
  3. This allows each component to autonomously evolve without worrying about how other componenets change. 
  4. Introduces messaging as a first class citizen within an application domain.
  5. Typical use cases include background processing using fire and forget mechanisms.
  
 ## Installation
 Nuget package is avaialble on nuget.org
 
 Using nuget package manager or the console you can add the nuget to your project. 

 ```
 PM> Install-Package Talkatives.Extensions.Messaging.Dataflow -Version 1.0.0
 ```

 ## Getting Started
 
 1. Add the nuget package Talkatives.Extensions.Messaging.Dataflow and register the publisher in the pub component and the subscriber in the sub component

 2. Setup Publisher

 ```csharp
 //Registering the publisher. Note that while registering you dont need to specify the type of publisher
 services.RegisterGenericInProcPublisher(new InprocMessageBusConfiguration
            {
                PublisherQueueSize = 100,
                PublishTimeoutMSec = 10000
            });



  //Resolving the publisher and publishing a string message
  var bus = _serviceProvider.GetService<IInprocMessageBus<string>>();
  bus.Publish($"Hello");

  //Resolving the publisher and publishing a PersonAdded event
    var bus = _serviceProvider.GetService<IInprocMessageBus<PersonAdded>>();
  bus.Publish(new PersonAdded {...});
 ```
 
 3. Setup subscriber

 ```csharp
 //Inherit from IInprocMessageSubscriber to implement consumers
public class Consumer01<string> : IInprocMessageSubscriber<string>
{
    public async Task OnNextAsync(string message)
        {
          //message processing logic goes here...


public class Consumer02<string> : IInprocMessageSubscriber<string>
{
    public async Task OnNextAsync(string message)
        {
          //message processing logic goes here...



//Register one or more consumers by type of messages being published
services.AddSingleton<IInprocMessageSubscriber<string>, Consumer01<string>>();
services.AddSingleton<IInprocMessageSubscriber<string>, Consumer02<string>>();
 ```

 ## Contributions
 PRs and feedback are welcome!

