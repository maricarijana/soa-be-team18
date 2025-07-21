/****** PAYMENTS MODULE ******/

DELETE FROM payments."PaymentRecords";
DELETE FROM payments."PurchaseTokens";
DELETE FROM payments."OrderItems";
DELETE FROM payments."ShoppingCarts";
DELETE FROM payments."Coupons";
DELETE FROM payments."Bundles";

/* ShoppingCarts */



/* OrderItems */



/* PurchaseTokens */


/* PaymentRecords */


/* Coupons */


/* Bundles */

INSERT INTO payments."Bundles"(
	"Id", "Name", "Price", "Status", "TourIds", "AuthorId")
	VALUES 
	  (225, 'Bundle1', 55, 1, ARRAY[225, 226]::integer[], 227);
INSERT INTO payments."Bundles"(
	"Id", "Name", "Price", "Status", "TourIds", "AuthorId")
	VALUES 
	 (226, 'Bundle1', 35, 1, ARRAY[226]::integer[], 227);


