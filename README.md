This was an honors project with my Distributed Software Development professor in Spring 2023.


This is a C# program with three components:
1. A REST API to scrape yahoo finance and return JSON formatted data about selected stocks based on their ticker symbols.
2. A REST API which does the above but returns XML formatted data.
3. A program to call the two REST APIs and display the results with an ASPX web form.

Note that because this project hosts the REST APIs locally, you may need to change the port numbers in the API calls in order to get this to work on your machine.

Additionally, this uses yahoo finance's HTML from when the project was done, and may not work properly with updates to yahoo finance's layout.
