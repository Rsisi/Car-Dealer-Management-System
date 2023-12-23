# Car-Dealer-Management-System
Built an auto dealership website supporting updating/searching inventory, tracking sales, and maintaining repair history functionalities with ASP.NET Razor frontend framework.

## Project Structure
Developed dealer backend following MVC design patter

- **Models** folder as class constructors for controler folder
- **View** folder contains all of the HTML pages for the supporting updating/searching inventory, tracking sales, and maintaining repair history functionalities
- **Controller** folders contains all the c# implemntation for all of the functinality for checking reports, updating/searching inventory, tracking sales, and maintaining repair history

## Project Details

### Search Vehicle

- Display number of Unsold Vehicle.
- If Click Search Dropdown – Display dropdown selections to Search by Vehicle Type, Manufacturer, Color or Year; 
- if User is logged in: Display option to search by VIN.

### Log In

- Click Login - Display Login Form.
- User enters username, password input fields.
- If data validation is successful for username and password input fields, go to Search Vehicle and display functions according to User. permission. 
- If fail, display Login Form, with error message.

### Lookup Customer

- User enters Customer input fields.
- If data validation is successful for input fields, add customer
- If fail, display error message.

### Add Customer

 - Click Select
 - Disply individual/business info form 
 - if fail, display erro message
 - User Enters input fields of displayed Info Form.
 - Add custoner

 ### View Vehicle Detail

- View Vehicle Detail
- Display VIN, Manufacturer, Model Year, Model Type, Vehicle Type, Vehicle Color, List Price, Description
- Find the Vehicle Type by Vehicle Type Name, Display corresponding attributes(like roof type and back seat count for convertible)

### Add Vehicle 
 - Populate Manufacturer and Vehicle Type dropdowns
 - While no buttons are pushed, do nothing.
 - If **Back to Search**, go to Access Form
 - If **Add Vechicle**, go to the **Vechicle Detail Form**.

 ### Start New Repair Order
 - Search by VIN in Sales Transaction table

 ### Update&Complete Repair

 - Display Repair Details.oDisplay the Repair Details.
 - When Add labor charge button is click, Enter the labor charge, Display the Repair Details.
 - When Add Part button is click, enter details. 

 ### Report Form
 - User login from the Login Form
 - Display **View Report** when the user's type id Manager or Owner.
 - If User click view report dropdown list for Report Request, display dropdown list of selections detail for the user.
 - The selectioins include: Sales by Color Report, Sales by Type Report, Sales by Manufacturer Report, Gross Customer Income Report, Repairs by Manufacturers/Type/Model Report, Below Cost Sales Report, Average Time in Inventory Report, Parts Statics Report, Monthly Sales Report.