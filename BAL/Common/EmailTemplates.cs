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
                                                    <h2 align='center'>Order Number : [[OrderId]]</h2>
                                                    <table border='2'>
                                                    <tr>
                                                    <td> S.No </td>
                                                    <td> Product </td>
                                                    <td> Product Name </td>
                                                    <td> Price </td>
                                                    <td> Quantity </td>
                                                    <td> Total Price </td>
                                                    </tr>
                                                    <tr>
                                                    <td> {0} </td>
                                                    <td> <img src='{1}' width='150px' height='100px' /> </td>
                                                    <td> {2} </td>
                                                    <td> {3} </td>
                                                    <td> {4} </td>
                                                    <td> {5} </td>
                                                    </tr>
                                                    </table>
                                                    </body>
                                                    </html>";
    }
}
