# Utb.PizzaKiosk

Pizzeria Self-Service Kiosk - Student project for AP3AF and AK3AF courses.

## Learning Objectives

### Audience

The audience is the students who have studied basic programming and OOP courses. Students are not expected to have experience with software projects.

### Goal

We want the students to apply their knowledge and comprehension of course topics on the semestral project so that students can use this project as a template for their own applications. 

### Learning outcomes

#### Knowledge

-	The **student** lists client-side frameworks.
-	The **student** lists backend frameworks.

#### Comprehension

-	The **student** explains the Model-View-ViewModel design pattern for creating a user interface.
-	The **student** explains the concepts of object serialization and deserialization.
-	The **student** explains how to access a relational database in code.
-	The **student** explains the concept of object-relational mapping (ORM).
-	The **student** explains how to call a web API on a client application.
-	The **student** justifies the benefits of separating the code for creating the user interface from the actual application logic.

#### Application

-	The **student** applies the Model-View-ViewModel design pattern to the user interface design.
-	The **student** defines the user interface independently of the application logic.
-	The **student** serializes and deserializes objects.
-	The **student** accesses relational database in code.
-	The **student** uses an object-relational mapping (ORM) library.
-	The **student** calls a web API on a client application.

## Software Requirements

### Glossary

- A **kiosk** is a hardware device with touch display that a customer uses for ordering a pizza.
- A **kiosk session** is a personalized ordering experience for a customer. 
- An **order fulfillment option** is a way in which the order is delivered and served to the customer.
- A **pizza** is a dish that is in the pizzeria menu.
- A **pizza’s option** is a choice for a pizza including toppings, crust types, sauces, and other customizable features.
- A **pizza's selection** is a list of unique options available for a specific kind of pizza.
- A **pizza’s configuration** is a list of chosen options by the customer for a specific kind of pizza.
- A **pizza menu** is a distinct, non-empty list of various types of pizza and their configurations.
- A **shopping cart** is a list of pizzas and their configurations chosen by the customer.
- An **order** is a non-empty list containing duplicate of pizzas and their configurations from the shopping cart, along with the current state of the order.

### Roles

- A **customer** is an individual or group that would like to order and configure pizzas and eat it in the restaurant or take it away. 
- An **operator** periodically checks for orders and process orders. 
- A **manager** prepares pizza menu for costomers. 

### User Requirements

#### The customer

**Context: Kiosk Session:**

1. Customer arriving at the kiosk:
   - The **customer** wants to have the kiosk ready for them.
2. Initialing a new session:
   - The **customer** wants to specify the order fulfillment options and begin the new personalized kiosk session.

**Context: Pizza selection:**

1. Selecting a Pizza:
   - The **customer** wants to navigate through the menu easily to select their desired options.
   - The **customer** wants to have the same menu during the kiosk session.
   - The **customer** wants to select the desired pizza from the pizza menu.
2. Pizza configuration
   - After selecting a pizza, the **customer** wants to configure selected pizza’s configuration.
     
**Context: Cart Interaction:**

1. Adding Pizzas to Cart:
   - After configuring pizza’s configuration, the **customer** wants to add pizzas and their configurations to the cart.
2. Removing Pizzas from Cart
   - Alternatively, if **the customer** decides, they want to delete all pizzas and their configuration from the cart.

**Context: Order**

1. Placing an Order:
   - The **customer** wants to order pizza and its pizza’s configurations in the cart.
2. Reviewing Order and Cost:
   - After placing the order, the **customer** wants to see the total order cost and receive a summary before placing the final order.
3. Confirming Order:
   - After reviewing the order and its cost, the **customer** can confirm the order.
   - Alternatively, the **customer** can cancel the order, but still wants to have pizzas in the cart and return to the pizza selection.

#### The operator

**Context: Order processing:**
   - The **operator** wants to see the list of non-empty multiset of orders ordered by date ascending.
   - After reviewing the list of order, the **operator** complete the order.
   - Or alternatively, the **operator** can cancel the order.

#### The manager

**Context: Pizza Menu:**

1. Preparing Menu:
   - The **manager** can create menu items, pizza descriptions, and configuration options.
   - Alternatively, the manager can make minor changes to items to correct typos and errors.
2. Archiving Menu Item:
   - The **manager** can archive menu item to make it invisible to customers, but keeping it available for administrative purposes. 

### Functional requirements

TODO: Rewrite user requirements to describe it for developer.

### Non-Functional requirements.

TODO: Check sentences in chatgpt.

- Usability: The user interface should be intuitive and easy to navigate.
- Localization: one language only, English or Czech.
- Accessibility: application should support or in near future supports individuals with disabilities, adhering to accessibility guidelines.
- Security: Access to operator and manager functionalities shall be secured through authentication and authorization.
- Compatibility: Client-side Multiplatform applications (Windows, Linux, Mac).
- Source code: Easy and readable code, essential code only.
- Testing: The system shall support time simulation for testing purposes.

### Constraints

- Tests: Only one example of unit test to make project achievable in time.
- Payment: No payment implementation, just order confirmation to make project easier.
- Local database so that students do not need to setup server.
- Web API for the backend pre-implemented by a teacher.
- Latest standard .NET core

### Data Requirements

A kiosk session:
- States: ready, inuse.
- Fulfillment options: Dine-in, Takeout, Delivery.

// TODO rewrite according to UML diagram types

The pizza description example:
- Name: PEPPERONI PIZZA,
- Description: Tomato base shredded mozzarella.

Pizza's selection example:
- Pizza options:
   - Size: Small, Medium (default), Large.
- Additional toppings (unsigned number):
   -  Onion: +5
   -  Jalapeños: +3

The pizza’s configuration choosen by the customer:
- List values 
- Bolean values
- Numeric values

### Architecture and Design

TODO: Rewrite as complete sentences.

- Desktop application with touch support for a customer, an operator and for a manager.
- Model-View-ViewModel (MVVM) design pattern for user interface.
- Software uses the Web API that access local database (Sqlite) using the Object-relational mapping (ORM).
- Separation of User Interface and Application Logic.
- Time simulation for testing.

#### Class diagrams

note: `PizzaIngredient` is a join table in a Many (`Pizza`) to Many (`Ingredient`) relation with payload `MinimalQuantity` and `FreeQuantity`.

```mermaid
classDiagram

   class OrderStatusType{
      <<enumeration>>
      Processing
      Ready
      Served,
      Cancelled
   }

   class FulfillmentOptionType{
      <<enumeration>>
      DineIn
      Takeout
   }

   class Pizza {
      <<Entity>>
      +Id : int
      +Name : string
      +Description : string
      +Price : decimal
      +IsAvailable : bool
      +AlergensList : string
   }

   

   class PizzaIngredient {
      <<Entity>>
      +PizzaId : int
      +IngredietId : int
      +MinimalQuantity : int
      +FreeQuantity: int
   }

  class Ingredient {
      <<Entity>>
      +Id : int
      +Name: string
      +QuantityDescription : string
      +UnitPrice: decimal
      +AlergensList : string
  }

  class Order {
      <<Entity>>
      +Id : int
      +Status : OrderStatusType
      +FulfillmentOption : FulfillmentOptionType
      +TotalPrice : decimal
   }

   class OrderedPizza {
      <<Entity>>
      +Id : int
      +OrderId : int
      +Name : string
      +TotalPrice : decimal
   }

   class OrderedIngredient {
      <<Entity>>
      +Id : int
      +OrderedPizzaId : int
      +Name : string
      +QuantityDescription : string
      +UnitPrice: decimal
      +FreeQuantity: int
      +PaidQuantity: int
      +TotalPrice : decimal
   }

   Pizza -- PizzaIngredient
   PizzaIngredient -- Ingredient
   Order --> OrderedPizza
   OrderedPizza --> OrderedIngredient

```

Customer App

```mermaid
classDiagram

   class KioskSession {
      <<Object>>
      +FulfillmentOption : FulfillmentOptionType
      +Cart : ShopingCart
      +PizzasInMenu : List~Pizzas~
      +SelectedPizza : PurchasePizza
   }

   class CartStatusType{
      <<enumeration>>
      InCart
      PendingCheckout
   }

   class ShopingCart {
      <<Object>>
      +Status : CartStatusType
      +CartPizzas: List~PurchasePizza~
   }
```

#### Implementation notes

- [Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli).
- [Testing against your production database system](https://learn.microsoft.com/en-us/ef/core/testing/testing-with-the-database).
- [Tutorial: Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-7.0&tabs=visual-studio).
- [Unit and integration tests in Minimal API apps](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/test-min-api?view=aspnetcore-7.0).
- [Value Conversions EF Core](https://learn.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations).
- [EF Inheritance](https://learn.microsoft.com/en-us/ef/core/modeling/inheritance).


---
## Acknowledgments 
- I would like to thank Luis Antonio Beltran for helping me with this project and for his deep knowledge that he has shared with me about MVVM and MAUI.
- While this assignment is the result of my (Erik Král) learning and creative effort, I acknowledge that the GPT-3.5 language model developed by OpenAI's guidance has deepened my understanding of topics related to software requirements, UML diagrams, and project management. I also appreciate that the GPT-3.5 model has assisted me in improving English grammar and sentence structuring.
