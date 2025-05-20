Simple web app that interacts with an SQL database.
The web app can be used for various Azure Test scenarios.

Web App:
![Screenshot 1](images/TestWebApp.jpg)


Tested with the following architectures:
- Multi-region App Service app approaches for disaster recovery -   
  - Active/Active
  - Active/Passive

![Screenshot 2](images/WebAppTopology.jpg)

A Web Application most likely depends on other services such as Azure SQL Database, Azure Storage accounts, or other. 
When designing a DR strategy for a Web app, each of these dependendent Azure services should be considered in the DR strategy.

References:
https://learn.microsoft.com/en-us/azure/architecture/web-apps/guides/multi-region-app-service/multi-region-app-service?tabs=paired-regions