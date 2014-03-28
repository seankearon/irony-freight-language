<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NewExampleSite.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FL to JavaScript compilation</title>
    <script type="text/javascript">

        function calculateFreight() {

            var region = getSelectedValue('regionDropdown');
            var customer = getSelectedValue('customerDropdown');

            var items = [];
            for (var i = 0; i < 4; i++) {
                var quantity = document.getElementById('quantity' + i).value;
                var weight = document.getElementById('weight' + i).value;
                items.push({ "weight": weight, "quantity": quantity });
            }

            var freight = getFreightCost(customer, region, items);

            alert('Freight cost is $' + (Math.round(freight * 100) / 100));

        }

        function getSelectedValue(selectId) {
            var dropdown = document.getElementById(selectId);
            return dropdown.options[dropdown.selectedIndex].value;
        }


    </script>
    <style type="text/css">
		body 
		{
			font-family: Arial;
		}
		h1 
		{
			color: Navy;
		}
		td input 
		{
			width: 30px;
		}
		
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
		<h1>Freight Language Demo Page</h1>
		
		<p>To see what this page is about, <a href="">view the article</a>.</p>
    
		<h2>Input</h2>
		
		<p id="errorMessageParagraph" style="font-weight:bold;color:Red" runat="server" visible="false" enableviewstate="false"></p>
		
		<asp:TextBox ID="sourceCodeTextBox" runat="server" Rows="18" TextMode="MultiLine" 
			Width="90%"></asp:TextBox><br />
		<asp:Button ID="runButton" runat="server" Text="Convert to JavaScript" onclick="runButton_Click" />
		
		<h2>Converted to JavaScript:</h2>
		
		<div>
			<asp:TextBox ID="javascriptTextBox" runat="server" Rows="18" TextMode="MultiLine" 
				Width="90%" ReadOnly="true"></asp:TextBox><br />
		</div>
		
		<h2>Sample input</h2>
		
		<div>
			Customer type: 
			<select id="customerDropdown"><option value="vip">VIP</option><option value="normal">Normal</option></select>
		</div>
		
		<div>
			Delivery region: 
			<select id="regionDropdown"><option value="africa">Africa</option><option value="asia">Asia</option><option value="Americas">Americas</option><option value="europe">Europe</option><option value="middleeast">Middle East</option></select>
		</div>
		
		<div>
			Products ordered:
			<table>
				<thead>
					<tr>
						<th>Quantity</th>
						<th>Product Name</th>
						<th>Weight</th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><input type="text" id="quantity0" value="1" /></td>
						<td>External drive</td>
						<td><input type="hidden" id="weight0" value="1.2" />1.2</td>
					</tr>
					<tr>
						<td><input type="text" id="quantity1" value="1" /></td>
						<td>Keyboard</td>
						<td><input type="hidden" id="weight1" value="0.2" />0.2</td>
					</tr>
					<tr>
						<td><input type="text" id="quantity2" value="1" /></td>
						<td>Computer</td>
						<td><input type="hidden" id="weight2" value="10.4" />10.4</td>
					</tr>
					<tr>
						<td><input type="text" id="quantity3" value="1" /></td>
						<td>Monitor</td>
						<td><input type="hidden" id="weight3" value="5.3" />5.3</td>
					</tr>
				</tbody>
			</table>
			
		</div>
		
		<div>
			<input type="button" onclick="calculateFreight();" value="Calculate freight" />
		</div>
		
    </div>
    </form>
</body>
</html>
