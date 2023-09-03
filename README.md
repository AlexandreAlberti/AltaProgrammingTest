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
- During the week I did a bit more of prep work and repaired my computer perfectly. No error messages now. I'm not comfortable using my work Macbook for this purpose, just I don't feel it's a right thing to do. So I'll face the pros/cons side of still using my own old PC.
- In this revision of the test task I upgraded to net5.0, instead of netcoreapp3.1.
- I added Dockerfile and some github Actions to allow usage of containers and automatic releases on master branch pushes. 
- Also added a diagram on how to deploy to allow creating multiple instances of server. ![Diagram](https://github.com/AlexandreAlberti/AltaProgrammingTest/blob/master/Diagram.png) The main idea is to have a load balancer for request, which has to know all up and running server instances. Using a tool like AWS Cloudformation (https://aws.amazon.com/cloudformation), we can dinamically set up the amount of needed server instances and deploy more or less based on usage, also it offers possibility to configure Load Balancer (https://aws.amazon.com/elasticloadbalancing/). I assume that we all know how this works. AWS already offers a way for Balancer to know all up and running instances, so configuration is minimal. I also assume that any other cloud provider will have similar tools. AWS is one that I used when worked at Adevinta/Schibsted.
- This things mentioned before can make the first work on the test without taking the feedback already received into account.

# Edit 2
- Now I'm taking the feedback into account.
- Added more specific update fields methods on Service's Layer with non critical values. Also, as we can update some fields on a property at once with some checks and using an enum Loop with a SET kind to avoid duplications in the loop and avoid updating other values (I decided instead of only updating some values because it's easier to understand we only want to update some fields, when it becomes old code, will be easier to understand) and better to have something like this to avoid doing several writes if we create a lot of update methods, 1 per field. (on accounts service, as we can only update names on requests I feel like that was a good enough way).
- Removed all var properties type, as suggested. I am really used into using proper variable names in Java at the moment. Just that when I do some stuff on c++ client-side to help, we have just the opposite rule, using auto all the time. I'm happy to know you put stricter definitions, in my opinion, that helps a lot how to understand the code better. 
- Renamed a bit variables to be more verbose.
- Created method "transaction" in purchasesController to allow move property ownership between user accounts. Also with a flag to tell they used custom price, so we can update balance of both users accordingly if that's the case. 
- Taking a look at the "Extra API features" I don't understand how it's supposed to work the fluctuations in prices. I only came up with the idea to create a scheduled job that, with some entry parameters like a range (0.7-1.3) gets a factor on each property to apply to buy and dell prices. (Already read a bit https://learn.microsoft.com/es-es/dotnet/api/system.threading.tasks.taskscheduler?view=net-5.0 documentation, seems fair enough)
- With the repeated name check, I tried to create an alternate key into Name field to be able to do fast checks if name already exists in database. As seems that in-memory db has some limitations with UNIQUE contraints and searches through Alternate Keys (read in some comments on StackOverflow), I decided to simulate it with another parallel Set called AccountNames.
- I created a TransactionLog Entity into another DbSet to store property changes logs. I only stored accounts ID's from buyers/sellers + a timestamp to be able to order them through time. I decided not to include buy/sell prices because that seems a bit too much info for the test purpose.

Again, thanks a lot for the chance to develop this a bit more and give a better idea how I can work with a stack I never used professionally before.