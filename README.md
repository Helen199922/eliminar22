# Actualizar Scaffold con nueva data Localmente

Scaffold-DbContext "server=localhost;database=dbname;user=user;password=pass" Pomelo.EntityFrameworkCore.MySql -OutputDir ModelsEF -Context "DBContext" -d -force


# Actualizar Scaffold con nueva data en la DB servidor
Scaffold-DbContext "Server=carniceria-zamorano-db.mysql.database.azure.com;UserID = zamoranoservidor;Password=12345678#!C0rn3;Database=carniceria" Pomelo.EntityFrameworkCore.MySql -OutputDir ModelsEF -Context "DBContext" -d -force