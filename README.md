# InProcessMessaging
InProcessMessaging defines a lightweight infrastructural component that allows different components within a system to interact in a messaging (pub/sub) fashion as opposed to using dependency injection and resolution.  
  1. This promotes loose coupling between various components wich allows for asynchronously communication within a process space.  
  2. This can also be extended across process spaces using forwarding mechanisms. 
  3. This allows each component to autonomously evolve without worrying about how other componenets change. 
  4. Introduces messaging as a first class citizen within an application domain.
  5. Typical use cases include background processing using fire and forget mechanisms.

 
