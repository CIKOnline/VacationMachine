# VacationMachine
## 2nd Edition of Refactoring Contest

## Problems:

- whole logic is in one method
- hard to test and introducing new business logic is hard and may breaks other logic
- poor database logic --> retreiving and afterwords storing can lead to breaking changes


## Solution:

- Moved Result Calculation to seperate class
- Moved magic strings to COnfiguration.cs
- Created Employee Class for retreiving and storing
- Created tests for all relevant parts (result calc, sending of mail, message and escalation manager)
- database needed logic to add up the days in one method (save) save retreives an employee and the employee receives a list of changes instead of an int of daysTaken