Though I have some experience with coding in Python, I’ve never actually done TDD in it. I wanted to see if GitHub Copilot could help me with this.

To start off, I needed something to build. Top of mind from the trusty ’list of example projects’ came a system for personal finance administration. I have some previous experience with building such a system, and got a good idea of how I would want that to work.

I started off by describing the first functionality, and instructing Copilot Chat to generate a test for it:

```plaintext
Help me create the unit tests for a personal finance administration system in Python.
Start with tests for the code that can allow the user to enter a transaction. 
The system will need to prompt the user for input on each property of the transaction. 
A transaction consists of the following fields:

- Date
- Time
- Description
- Amount
- Category

```

The prompt, as usual, resulted in a broad step by step instruction on how to implement this, followed by the actual code. Which looked something like this:

```python
import unittest
from unittest.mock import patch
import finance_system # Assuming this is the module with your finance system

class TestTransactionEntry(unittest.TestCase):
 @patch('builtins.input', side_effect=['2023-04-01', '12:00', 'Lunch', '15.00', 'Food'])
 def test_add_transaction(self, mock_inputs):
 # Assuming finance_system has a function add_transaction() that returns True if the transaction is added successfully
 result = finance_system.add_transaction()
 self.assertTrue(result, "The transaction should be added successfully.")

if __name__ == '__main__':
 unittest.main()

```

At first glance, it looks like a passable test. There is one test case, and that is perfectly fine for now. It tests if calling an `add_transaction` function with the correct inputs will return `True`.

Running the test of course results in a failure. We did not write any implementation yet.

![Test failure](https://sswart.github.io/images/python-tdd-copilot/test-failure.png)

I then proceeded to implement the `add_transaction` function. I instructed Copilot to generate the implementation for me:

```plaintext
Implement the finance_system module and add_transaction function for me

```

The resulting code was straightforward:

```python
# finance_system.py

class Transaction:
 def __init__(self, date, time, description, amount, category):
 self.date = date
 self.time = time
 self.description = description
 self.amount = amount
 self.category = category

# Assuming transactions are stored in a list
transactions = []

def add_transaction():
 date = input("Enter the transaction date (YYYY-MM-DD): ")
 time = input("Enter the transaction time (HH:MM): ")
 description = input("Enter the transaction description: ")
 amount = input("Enter the transaction amount: ")
 category = input("Enter the transaction category: ")
 
 # Create a new Transaction instance
 transaction = Transaction(date, time, description, amount, category)
 
 # Add the transaction to the list
 transactions.append(transaction)
 
 # For simplicity, return True to indicate success
 return True

```

I ran the test again, and this time it passed!

![Test success](https://sswart.github.io/images/python-tdd-copilot/test-success.png)

It was time to get back to the tests.

```plaintext
Add edge cases for things like invalid date input, invalid time input. Also add tests that checks if the description has a maximum of 50 characters and the category has a maximum of 20 characters

```

Copilot came up with the following test cases:

```python
@patch('builtins.input', side_effect=['2023/04/01', '12:00', 'Lunch', '15.00', 'Food'])
def test_invalid_date_format(self, mock_inputs):
 with self.assertRaises(ValueError):
 finance_system.add_transaction()

@patch('builtins.input', side_effect=['2023-04-01', '1200', 'Lunch', '15.00', 'Food'])
def test_invalid_time_format(self, mock_inputs):
 with self.assertRaises(ValueError):
 finance_system.add_transaction()

@patch('builtins.input', side_effect=['2023-04-01', '12:00', 'L' * 51, '15.00', 'Food'])
def test_description_length_exceeds_limit(self, mock_inputs):
 with self.assertRaises(ValueError):
 finance_system.add_transaction()

@patch('builtins.input', side_effect=['2023-04-01', '12:00', 'Lunch', '15.00', 'F' * 21])
def test_category_length_exceeds_limit(self, mock_inputs):
 with self.assertRaises(ValueError):
 finance_system.add_transaction()

```

Running the tests of course resulted in failures. I then proceeded to have copilot implement the checks in the `add_transaction` function. At this point I needed to start thinking about what context is available to Copilot Chat. Not only does it need to know about the contents of my tests file, it also needs to know about the contents of my `finance_system` module so that it can match the implementation to the tests. Using `#file` to refer to both files did the trick:

```plaintext
Implement the functionality described by the tests #file:test.py #file:finance_system.py

```

```python
def add_transaction():
 date_input = input("Enter the transaction date (YYYY-MM-DD): ")
 time_input = input("Enter the transaction time (HH:MM): ")
 description = input("Enter the transaction description: ")
 amount = input("Enter the transaction amount: ")
 category = input("Enter the transaction category: ")
 
 # Validate date
 try:
 datetime.datetime.strptime(date_input, '%Y-%m-%d')
 except ValueError:
 raise ValueError("Invalid date format. Please use YYYY-MM-DD.")
 
 # Validate time
 try:
 datetime.datetime.strptime(time_input, '%H:%M')
 except ValueError:
 raise ValueError("Invalid time format. Please use HH:MM.")
 
 # Validate description length
 if len(description) > 50:
 raise ValueError("Description exceeds 50 characters.")
 
 # Validate category length
 if len(category) > 20:
 raise ValueError("Category exceeds 20 characters.")
 
 # Create and add the transaction
 transaction = Transaction(date_input, time_input, description, amount, category)
 transactions.append(transaction)
 
 return True

```

The suggestion was not very user friendly. Instead of providing feedback right away, the user would have to enter all values, and only after that be told that the input was invalid. Copilot needed some more pointers:

```plaintext
Do the validation for each input right after it is received so that the user can get instant feedback

```

```python
def add_transaction():
 date_input = input("Enter the transaction date (YYYY-MM-DD): ")
 # Validate date immediately
 try:
 datetime.datetime.strptime(date_input, '%Y-%m-%d')
 except ValueError:
 raise ValueError("Invalid date format. Please use YYYY-MM-DD.")
 
 time_input = input("Enter the transaction time (HH:MM): ")
 # Validate time immediately
 try:
 datetime.datetime.strptime(time_input, '%H:%M')
 except ValueError:
 raise ValueError("Invalid time format. Please use HH:MM.")
 
 description = input("Enter the transaction description: ")
 # Validate description length immediately
 if len(description) > 50:
 raise ValueError("Description exceeds 50 characters.")
 
 amount = input("Enter the transaction amount: ")
 # Amount validation can be added here if needed
 
 category = input("Enter the transaction category: ")
 # Validate category length immediately
 if len(category) > 20:
 raise ValueError("Category exceeds 20 characters.")
 
 # Create and add the transaction
 transaction = Transaction(date_input, time_input, description, amount, category)
 transactions.append(transaction)
 
 return True

```

The tests passed. Time to refactor. Of course, for challenges sake, I asked Copilot to do it for me.

```plaintext
Use a Value Object for each input to handle the validation

```

Now we are starting to get somewhere. We are ensuring we are using the correct types for our inputs and can now trust that any use of the input values in the future will be valid.

```python
import datetime

class TransactionDate:
 def __init__(self, date_str):
 try:
 self.value = datetime.datetime.strptime(date_str, '%Y-%m-%d').date()
 except ValueError:
 raise ValueError("Invalid date format. Please use YYYY-MM-DD.")

class TransactionTime:
 def __init__(self, time_str):
 try:
 self.value = datetime.datetime.strptime(time_str, '%H:%M').time()
 except ValueError:
 raise ValueError("Invalid time format. Please use HH:MM.")

class TransactionDescription:
 def __init__(self, description):
 if len(description) > 50:
 raise ValueError("Description exceeds 50 characters.")
 self.value = description

class TransactionAmount:
 def __init__(self, amount):
 # Assuming amount is a numeric value; add more validation as needed
 self.value = float(amount)

class TransactionCategory:
 def __init__(self, category):
 if len(category) > 20:
 raise ValueError("Category exceeds 20 characters.")
 self.value = category

class Transaction:
 def __init__(self, date, time, description, amount, category):
 self.date = date
 self.time = time
 self.description = description
 self.amount = amount
 self.category = category

transactions = []

def add_transaction():
 date_input = TransactionDate(input("Enter the transaction date (YYYY-MM-DD): "))
 time_input = TransactionTime(input("Enter the transaction time (HH:MM): "))
 description = TransactionDescription(input("Enter the transaction description: "))
 amount = TransactionAmount(input("Enter the transaction amount: "))
 category = TransactionCategory(input("Enter the transaction category: "))
 
 transaction = Transaction(date_input.value, time_input.value, description.value, amount.value, category.value)
 transactions.append(transaction)
 
 return True

```

One thing left to do is have the Transaction class also use the newly defined Value Objects:

```plaintext
Make the Transaction class use the newly created Value Objects

```

```python
class Transaction:
 def __init__(self, date: TransactionDate, time: TransactionTime, description: TransactionDescription, amount: TransactionAmount, category: TransactionCategory):
 self.date = date.value
 self.time = time.value
 self.description = description.value
 self.amount = amount.value
 self.category = category.value

def add_transaction():
 date_input = TransactionDate(input("Enter the transaction date (YYYY-MM-DD): "))
 time_input = TransactionTime(input("Enter the transaction time (HH:MM): "))
 description = TransactionDescription(input("Enter the transaction description: "))
 amount = TransactionAmount(input("Enter the transaction amount: "))
 category = TransactionCategory(input("Enter the transaction category: "))
 
 transaction = Transaction(date_input, time_input, description, amount, category)
 transactions.append(transaction)
 
 return True

```

The tests passed. I was happy with the result. I had a working system that I could build upon. I had a decent set of tests that I could trust.

This experiment showed me that even if I’m no expert in a language, I can get started developing with any language quickly with the help of Copilot, while even using a way of working that is familiar to me.