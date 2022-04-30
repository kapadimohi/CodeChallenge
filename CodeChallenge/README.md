# Code Challenge
This repository represents code that has been written as part of a code challenge. 

### Problem Statement
The task is to take in three pieces of data, payslips for an employee (employee code), payments (disbursements) made to a Super fund and a set of instructions of how to treat each type of payment (Pay Code)

For a company to correctly pay super for an employee they must pay their staff 9.5% of Pay Codes that are treated as Ordinary Time Earnings (OTE) within 28 days after the end of the quarter. Quarters run from Jan-March, Apr-June, Jul-Sept, Oct-Dec.

Super earned between 1st Jan and the 31 st of March (Q1) will need to be paid/Disbursed between the 29th Jan - 28th of Apr.

For example:

Pay Codes are:
- Salary = OTE
- Site Allowance = OTE
- Overtime - Weekend = Not OTE
- Super Withheld = Not OTE

Payslips are:

    Jan 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Overtime - Weekend, Amount $1500
    	- Code: Super Withheld = $475

    Feb 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Super Withheld = $475

    March 1st Payslip
    	- Code: Salary, Amount $5000
    	- Code: Super Withheld = $475


Disbursements:
- $500 on  27th Feb
- $500 on 30th March
- $500 on 30th Apr

For this Quarter we need to know the following:

	1) Total OTE = $15,000
	2) Total Super Payable = $1425
	3) Total Disbursed = $1000
	4) Variance = $425

### Challenge

Read in the sample file which contains the Disbursements, Payslips and Pay Codes.
For Each of the 4 employees show the Quarterly grouping for the OTE amount, Super Payable and the Disbursement totals

### Getting started

#### Running the Application 

Run the following command in the root directory. This will start up the webapi and AWS local stack with S3 configured and a sample disbursement file available for consumption

    docker-compose up    

Browse to the following url to display swagger to test the VerifyDisbursement functionality

    https://localhost:7272/swagger/index.html

Browse to the following url to test the VerifyDisbursement functionality

    https://localhost:8080/VerifyDisbursements

To debug, run debugger in the IDE while LocalStack is running

### Design Decisions

1. Used a WebApi to complete this challenge keeping in line with micro services architecture. A console application would have sufficed, however, ideally such an application would normally be an api i.e be consumed by a UI or another service. A lambda was also considered but not pursued due to complexity and scaling issues due to a max time limit of 15minutes, which would make it less than ideal in real work situations.


2. Approach to solving the business problem. 
   
    - Retrieving each set of the spreadsheet and mapping it into a different POCO
    - Avoided any business logic at the repository level and raw data might be use for other use cases currently not defined
    - PayslipDetails data was then grouped into Employee Code, Year and Quarter. DateTime extension method was created to easily identify the quarter for which the payslip was for. During this process, all the Non Ote transactions were filtered out of the dataset
    - Disbursements data was then also grouped into Employee Code, Year and Quarter. A different DateTime extension method was created to identify the quarter for which the payment was for. This is because of a business rule described in the challenge that even a payment made within 28 days into the next quarter, it should still be attributed to the previous quarter 
    - The goal was to have the same key for both sets of data (EmployeeCode, Year and Quarter) which then can be used to combine the data together using Linq or something similar
    - 


2. Use of AWS LocalStack. Initially the application just used local file system for IO. However, I decided to change it to use LocalStack instead since this will closely match the real work situation. Disbursement files are likely to be stored on S3 and its best to integrate with S3 locally as well. All that will be needed to make this work on Production is Production AWS credentials and S3 service URL. Rest should just work.


3. Implemented a simple 3 tier application. Program entry is via the controller which takes in the name of the file that needs to be assessed, which then hands over to a service which orchestrates the retrieval of multiple sheets of data from the repositories and business logic to calculate the Disbursement variances. 


4. Interfaces where possible. The use of interfaces is extensive to aid in unit testing and future extensibility. For e.g. IDataRepository is currently implemented by S3DataRepository. If tomorrow the repository location changes, it should be easy enough to create another class that implements the same interface and retrieves data from another source. 


5. Comments are kept at a minimum. Code should ideally be self documenting with good usage of variable and method names e.g. MergePayslipAndDisbursementData does exactly what it says. A couple of exceptions are in the SuperCalculator where it made sense to me to include all the historical and future rates of Super for reference purposes. Another place for comments are in the DateTime extensions. There are 2 different ways of calculating "Quarterly" for this application and I believe it made sense to differentiate them with comments since it is not obvious. 


6. Logging has also been used where appropriate to define entry points into controllers and other "important" functions. This helps with traceability/monitoring if things need to be debugged in production. 


7. 