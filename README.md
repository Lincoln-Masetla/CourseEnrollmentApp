# Course Enrollment App

This repository contains the source code for the Course Enrollment App, a web application that allows users to enroll in courses. The app follows the Clean Architecture principles to maintain a scalable and maintainable codebase.

## Clean Architecture

Clean Architecture was chosen for this project due to its numerous benefits, which include separation of concerns, scalability, testability, adaptability, easier maintenance, and loose coupling. These characteristics ensure a well-organized, manageable, and flexible codebase that simplifies development, testing, and future enhancements.

## Layers

The Clean Architecture principle is applied in this project, separating concerns into clear layers:

1. **Core**: Domain entities and business logic. The Core layer houses the essential elements of the domain, encapsulating business rules and defining models. This layer is central to the application and remains independent of external dependencies.

2. **Application**: Application-specific rules and coordination of the domain components. This layer is responsible for orchestrating the core components and implementing the application's workflows. It defines interfaces for interacting with external components, such as the Infrastructure layer, ensuring that the Core layer remains isolated from specific technology choices or implementation details.

3. **Infrastructure**: Data access and persistence mechanisms. The Infrastructure layer is responsible for concrete implementations of the interfaces defined in the Application layer, allowing the system to interact with external services, like databases or APIs. By maintaining the infrastructure code separately, it can be easily extended or replaced without affecting the core business logic.

4. **Web**: Presentation and user interaction components. This layer hosts the web frontend, including views, components, and controllers, to display data from the Application layer and allow user interactions. It is decoupled from the underlying business logic, making it straightforward to update the user interface or switch to a different presentation technology.

By organizing the project into these distinct layers, the application benefits from a clean, modular, and maintainable codebase.

## Components

- **CourseEnrollmentApp.Core**: Contains domain entities and interfaces for services and repositories.
- **CourseEnrollmentApp.Application**: Implementations of the application services, including any business logic.
- **CourseEnrollmentApp.Infrastructure**: Contains data access code and implementations of the repository interfaces.
- **CourseEnrollmentApp.Web**: The web frontend, including views, components, and controllers.
- **Tests**: Unit tests for the Application, Infrastructure, and Web layers.

  ![clean resized](https://github.com/user-attachments/assets/7f44d88a-2399-4ade-9b30-d033267d2062)

## Requirements

- .NET 8 SDK
- (Optional) IDE, e.g., Visual Studio or Visual Studio Code

## Setup and Run

To set up and run the project, follow these steps:

1. Clone the repository.
2. Open `CourseEnrollmentApp.sln` in Visual Studio.
3. Build and run the `CourseEnrollmentApp.Web` project.

## Usage
1. Create an Account.
2. Register for a course that interests you.
3. Deregister a course.
4. Logout / Login

## Testing

Each project component (Application, Infrastructure, and Web) has corresponding unit tests. To run the tests, open the "Test Explorer" in Visual Studio, and click "Run All".

