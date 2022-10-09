CIK
# VacationMachine
## 2nd Edition of Refactoring Contest

## Rules:

- Time - 3 weeks (10th - 28th October)	
- Task - Create one Pull Request where you refactor code from this repository
  - prepare new branch with your acronym  (refactor/[acronym] -  refactor/cik) 
  - Add description where you point out what is the main issue of this code.	
- Prize - Spooky Surprise Box!

## Task

This time we are taking in consideration service which handle holidays of employee topic.
Please take a look on class ```VacationService```. Over there you can find a method,
```RequestPaidDaysOff```. 
Logic behind decision can be describe like this:
1. If you are a high-performance employee ("PERFORMER"), You have
26 days that you can always use (Decision = Approved). Between 26 and 45
days requests can be accepted, but must first be
manually verified by an escalation manager (decision =
Manual). Any application that exceeds 45 days is rejected
(Decision = Denied). A rejection results in a corresponding
e-mail.

2. If you are a regular employee ("REGULAR"), applications over
26 days are rejected, up to 26 days are accepted. Rejection results in
rejection by sending an e-mail to that effect.


3. If you are a low-performing employee ("SLACKER"), all
applications are rejected, rejection will result in an appropriate
e-mail.

Acceptance of the request results in a new status (number of days) being saved in the database
and sending a message to the data bus.

## 1. Requirements

The most relevant logic is that which verifies that an employee with the
appropriate performance gets (or not) a holiday. We want to test it.

The escalation manager (```EscalationManager.cs```) is a paid service, we want to call it only when it is necessary. We want to be able to test it.

The email and email content are very important aspects proposed by the new Agile Coach - they are intended to influence the morale of people who would not go for holiday. We want to test this.


## 2. Definition of the problem

### Hints: 

1. Very frequent changes related to the infrastructure (database schema) causes the class to stop working
2. Maintenance of the tests and the class is very difficult - the level of its complexity rises.
3. Introducing new business rules (or changing them), e.g., regarding the number of holiday days, new employee statuses, is very long, tedious and error-prone. This is because directly next to it is code that talks to external components such as MessageBus or EscalationManager.

### Which aspects will be checked and rewarded:

Try to spot the problem, name it (describe it) and provide solution how to solve it.

## 3. Things which are not going to give you extra points

Main task is about refactoring. 

You are not going to get extra points for fireworks. 

It's not about introducing IoC or FluentValidator or put this service on Azure cloud. 

Try to focus on code!

## 4. Reward!

You can expect glory and Spooky Surprise Box for the best solution!!
