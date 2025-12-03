---
mode: 'agent'
description: 'Generate C# unit tests for selected functions or methods using xUnit and Moq'
---

## Task
Analyze the selected C# method or service and generate focused unit tests that thoroughly validate its behavior using xUnit and Moq.

## Test Generation Strategy

1. **Core Functionality Tests**
- Test expected behavior with typical input data
- Verify return values and state changes
- Use realistic domain examples (entities, DTOs)

2. **Input Validation Tests**
- null values
- empty strings
- invalid GUID
- boundary values

3. **Error Handling Tests**
- Expected exceptions (e.g. NotFoundException, ValidationException)
- Use Assert.ThrowsAsync
- Verify exception message

4. **Interaction / Side Effects Tests**
- Verify calls to repository or service dependencies
- Use Moq `Setup()` and `Verify()`
- Ensure no unexpected extra calls

## Test Structure Requirements

- **Testing Framework:** xUnit
- **Mocking Library:** Moq
- **Pattern:** AAA (Arrange / Act / Assert)
- One test class per target class
- One test method per scenario
- Use descriptive method names: MethodName_ShouldDoSomething_WhenCondition()

### Sample Test Structure

```
csharp
public class ClassNameTests
{
    private readonly Mock<IDependency> _mock;
    private readonly ClassName _service;

    public ClassNameTests()
    {
        _mock = new Mock<IDependency>();
        _service = new ClassName(_mock.Object);
    }

    [Fact]
    public async Task MethodName_ShouldDoSomething_WhenValid()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public async Task MethodName_ShouldThrowException_WhenInvalid()
    {
        // Arrange

        // Act & Assert
    }
}
```

## Inputs
Target function: CreateArticleAsync
Testing framework: xUnit + Moq
