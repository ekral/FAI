# Class Diagram for Microsoft eShop

Simplified UML Class diagram for Microsoft eShop Reference Application - "Northern Mountains":
https://github.com/dotnet/eShop

## Catalog API

```mermaid
classDiagram

   class CatalogItem{
      Id
      Name
      Description
      Price
      CatalogTypeId
      CatalogBrandId
      AvailableStock

   }

   class CatalogType{
      Id
      Type : string
      + RemoveStock(int quantityDesired) : int
      + AddStock(int quantity) : int 
   }

   class CatalogBrand{
      Id
      Brand : string
   }

   CatalogItem o -- CatalogType
   CatalogItem o -- CatalogBrand

```

## Basket API

```mermaid
classDiagram

   class CustomerBasket{
      BuyerId
      Items : ListBasketItem>

   }

   class BasketItem{
      Id
      ProductId
      ProductName
      UnitPrice
      OldUnitPrice
      Quantity
   }

   CustomerBasket o -- BasketItem
```

# Ordering API

```mermaid
class Buyer{
   Id
   Name
   PaymentsMethods : IEnumerable<PaymentMethod>
   + VerifyOrAddPaymentMehtod(CreditCardInfo creditcard) : PaymentMethod
}

class PaymentMethod{

   
}




```