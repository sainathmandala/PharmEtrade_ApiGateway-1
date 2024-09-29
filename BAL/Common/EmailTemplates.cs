using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Common
{
    public class EmailTemplates
    {
        public const string ORDER_TEMPLATE = @"<html>
                                                    <body>
                                                    <h1 align='center'>Thank you for your order</h1>
                                                    <h2 align='center'>Please find your order details below</h2>
                                                    <h2 align='center'>Order Number : {{OrderId}}</h2>
                                                    <table border='2' align='center' width='80%'>
                                                    <tr>
                                                    <td> S.No </td>
                                                    <td> Product </td>
                                                    <td> Product Name </td>
                                                    <td> Price </td>
                                                    <td> Quantity </td>
                                                    <td> Total Price </td>
                                                    </tr>
                                                    {{OrderDetailsHTML}}
                                                    </table>
                                                    </body>
                                                    </html>";


        public const string CUSTOMER_TEMPLATE = @"<html>
                                                     <body>
                                                      <h1 align='center'>Thank you for registration</h1>
                                                      <h2 align='center'> please chek you registration details/h2>
                                                      <h2 align= 'center'>Registraion ID :{{CustomerId}}
                                                      <table border='2' align='center' width='80%'>
                                                      <tr>
                                                      <td> FirstName </td>
                                                      <td> Email </td>
                                                      <td> Password </td>
                                                      <td> Mobile </td>
                                                      <td> CustomerTypeId </td>
                                                      <td> AccountTypeId </td>
                                                      </tr>
                                                      {{RegistrationDetailsHTML}}
                                                      </table>
                                                      </body>
                                                      </html>";
    }
}
