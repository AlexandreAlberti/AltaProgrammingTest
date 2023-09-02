# AltaProgrammingTest - From Alexandre Alberti

- I added a Service Layer bewteen Controllers and DB access. And also added some more data classes on Models folder.
- Created Upkeep method to be able to calculate new Account balance on demand. Added a min time to update of 5 min, to not overload DB saves, but skippable in critical ops, like selling or buying a property.
- Created an extra Controller/Service to manage buys/sells of properties.
- Added some more properties to manage ownership of propieties.
- I added only 1 test because it was difficult to set up with no knowledge on it, specially on the Mock part, and need to spend more time than I should on it (I'm used to do it in Java). Despite that, adding more cases should be easy from that point, specially would have liked to test the buy/sell cases. Testing the Upkeep of an account already allowed me to do some fixes to the code and buy/sell were tested with Postman with easy good results.
- Time used is around 5h (2.5 in the morning and 2.5 in the afternoon), with a lot of interruptions due to urgent things to do because of the bad weather.
- I was able to Run and test the application with MVS 2019, just clicking on RUN ApiProgrammingTest_Incomplete.
- For tests I need to use NuManager to get Moq included. Maybe you won't need to do that as it's on the .cdproj file, but just in case.
- It was a really long time since I last used MVS (in my master degree almost 6y ago), and almost same time since I do not program on a Windows machine.
- I'm still using an old Win7 Computer (Built in 2008) for personal usage, so newer version of MVS is not available, neither .Net 6. Also, Nvidia graphic driver is exploding quite often, so my screen gets black from time to time. Thankfully, still was able to complete the test. Just another difficulty.
- Thanks for the time spent evaluating my work. It was quite enjoyable to face this challange and I hope to be challanged more.

# Edit 1
I added Dockerfile and some github Actions to allow usage of containers and automatic releases on master branch pushes. Also added a diagram on how to deploy to allow creating multiple instances of server.
![Diagram](https://github.com/AlexandreAlberti/AltaProgrammingTest/assets/16921535/92a03388-29d2-4b42-a883-8fae71f75456)
The main idea is to have a load balancer for request, which has to know all up and running server instances. Using a tool like AWS Cloudformation (https://aws.amazon.com/cloudformation), we can dinamically set up the amount of needed server instances and deploy more or less based on usage, also it offers possibility to configure Load Balancer (https://aws.amazon.com/elasticloadbalancing/). I assume that we all know how this works. AWS already offers a way for Balancer to know all up and running instances, so configuration is minimal. I also assume that any other cloud provider will have similar tools. AWS is one that I used when worked at Adevinta/Schibsted.
