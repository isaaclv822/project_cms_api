---
mode: 'agent'
description: 'Generate unit tests for selected functions or methods'
---

## Task
Analyze the selected function/method and generate focused unit tests that thoroughly validate its behavior.

## Test Generation Strategy

1. **Core Functionality Tests**
- Test the main purpose/expected behavior
- Verify return values with typical inputs
- Test with realistic data scenarios

2. **Input Validation Tests**
- Invalid input types
- null / undefined values
- empty strings/arrays/objects
- boundary values (min/max&)

3. **Error Handling Tests**
- Expected exceptions
- Error messages
- Edge cases

4. **Side Effects Tests**
- External calls
- State changes
- Interaction with dependencies

## Test Structure Requirements

- Use the framework of the project (pytest, jest&)
- AAA Pattern : Arrange / Act / Assert
- Descriptive test names
- Group tests with describe/context blocks
- Mock external dependencies cleanly

Target function: ${input:function_name}
Testing framework: ${input:framework}