<h2>Luồng hoạt động: </h2><br>
<h3>Các request API hiện có:</h3><br>
- /api/Customer: Hiển thị toàn bộ thông tin khách hàng (Customer)<br>
- /api/Customer/{customerId} : tham số truyền vào là CustomerID, dựa vào tham số này sẽ lấy ra toàn bộ thông tin của 1 khách hàng có CustomerID đó <br>
- /api/selectOM : hiển thị toàn bộ các OrderMaster - mỗi một OrderMaster là một hóa đơn, bao gồm quan hệ với các OrderDetail gồm các thông tin tương ứng với hóa đơn đó <br>
- /api/selectOM/{omid} : tham số truyền vào là omid, dựa vào tham số omid (tức là OrderMasterID) để hiển thị toàn bộ thông tin của một hóa đơn<br>
- /api/EditOrder/updateOM/{rdid} : tham số truyền vào là rdid, dựa vào tham số rdid (tức là RowDetailID) để cập nhật (sửa) thông tin của một dòng sản phẩm bên trong hóa đơn <br>
- /api/EditOrder/updateOM/save : để lưu lại các thông tin như OrderNo, CustomerID, TotalAmount <br>
- /api/Info : Hiển thị tổng số lượng hóa đơn thông qua tổng số record của bảng OrderMaster <br>
- /api/Item: Hiển thị toàn bộ thông tin hàng hóa (Customer)<br>
- /api/Item/{itemId} : tham số truyền vào là itemId, dựa vào tham số này sẽ lấy ra toàn bộ thông tin của 1 hàng hóa có itemId đó <br>
- /api/OrderHandle : Hiển thị toàn bộ thông tin hóa đơn (OrderMaster) <br>
- /api/OrderHandle/init : Khởi tạo hóa đơn, lưu sẵn OrderMasterID, OrderDate và DivSubID vào bảng OrderMaster (các giá trị còn lại là 0)<br>
- /api/OrderHandle/add : Lưu lại thông tin của mỗi dòng sản phẩm vào hóa đơn có OrderMasterID đã được khởi tạo ở API: /api/OrderHandle/init <br>
- /api/OrderHandle/submit: Lưu lại thông tin của toàn bộ các dòng sản phẩm vào hóa đơn có OrderMasterID đã được khởi tạo ở API: /api/OrderHandle/init <br><br>
<h3>Ý tưởng hoạt động:</h3><br>
- Người dùng khởi tạo hóa đơn, đồng thời lưu ID của hóa đơn (OrderMasterID) vào Session (thông qua API: /api/OrderHandle/init) --> Sau đó người dùng thêm từng dòng sản phẩm vào hóa đơn này, lưu lại các dòng sản phẩm (thông qua API : /api/OrderHandle/add ) --> Submit lưu lại hóa đơn (thông qua API : /api/OrderHandle/submit ) <br><br>
- Cho phép người dùng sửa thông tin hóa đơn thông qua các API: /api/selectOM/{omid} ; /api/EditOrder/updateOM/{rdid} và /api/EditOrder/updateOM/save
<br><br>

- /api/selectOM/{omid} : hiển thị thông tin của một hóa đơn cụ thể nào đó, gồm có : ItemID, Quantity, Price, Amount <br>
- /api/EditOrder/updateOM/{rdid} : fromForm sửa lại các thông tin của hóa đơn, như: ItemID, Quantity, Price (từ form HTML truyền vào DTO)<br>

- /api/EditOrder/updateOM/save : lưu lại các thay đổi
