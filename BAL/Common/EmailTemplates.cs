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
                                                      <h1 align='center'>Thank you for registering with us</h1>
                                                      <h2 align='center'>Your Account is under Review.</h2>
                                                      <h3 align= 'center'>Your Registration ID :{{CustomerId}} </h3>
                                                      <h3 align= 'center'>You will get notified once your account is active. </h3>
                                                      <br /><br />
                                                      <table border='0'>
                                                      <tr>
                                                      <td> <b>User Id</b> </td>
                                                      <td> {{CUST_EMAIL}} </td>
                                                      </tr>
                                                      <tr>
                                                      <td> <b>Full Name</b> </td>
                                                      <td> {{CUST_FULL_NAME}} </td>                                                      
                                                      </tr>
                                                      <tr>
                                                      <td colspan='4'>
                                                      <br /><br /><br /><br /><br /><br /><br /><br /><br />
                                                      <img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='200px' height='45px' /> <br />
                                                      <h3>Team - PharmETrade</h3>
                                                      </td>
                                                      </tr>
                                                      </table>
                                                      </body>
                                                      </html>";
        public const string CUSTOMER_EDIT_TEMPLATE = @"<html>
                                                     <body>
                                                      <h1 align='center'>Profile updated Successfully</h1>
                                                      <h2 align= 'center'>Registration ID :{{CustomerId}}
                                                      <br /><br />
                                                      <table border='0'>
                                                      <tr>
                                                      <td> <b>User Id</b> </td>
                                                      <td> {{CUST_EMAIL}} </td>
                                                      </tr>
                                                      <tr>
                                                      <td> <b>Full Name</b> </td>
                                                      <td> {{CUST_FULL_NAME}} </td>                                                      
                                                      </tr>
                                                      <tr>
                                                      <td colspan='4'>
                                                      <br /><br /><br /><br /><br /><br /><br /><br /><br />
                                                      <img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='200px' height='45px' /> <br />
                                                      <h3>Team - PharmETrade</h3>
                                                      </td>
                                                      </tr>
                                                      </table>
                                                      </body>
                                                      </html>";
        public const string CUSTOMER_ACTIVATE_DEACTIVATE_TEMPLATE = @"<html>
                                                     <body>
                                                      <h1 align='center'>Your account has been {{CUST_STATUS}}</h1>
                                                      <h2 align= 'center'>Registration ID :{{CustomerId}}
                                                      <br /><br />
                                                      <table border='0'>
                                                      <tr>
                                                      <td> <b>User Id</b> </td>
                                                      <td> {{CUST_EMAIL}} </td>
                                                      </tr>
                                                      <tr>
                                                      <td> <b>Full Name</b> </td>
                                                      <td> {{CUST_FULL_NAME}} </td>                                                      
                                                      </tr>
                                                      <tr>
                                                      <td colspan='4'>
                                                      <br /><br /><br /><br /><br /><br /><br /><br /><br />
                                                      <img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='200px' height='45px' /> <br />
                                                      <h3>Team - PharmETrade</h3>
                                                      </td>
                                                      </tr>
                                                      </table>
                                                      </body>
                                                      </html>";

        public const string CUSTOMER_INVOICE = @"<html>
                                                    <body>
                                                    <table border='1' align='center' width='90%'>
                                                    <tr>
                                                    <td>
                                                    <table border='0' align='center' width='95%'>
                                                    <tr>
                                                    <td align='left' margin='10'> 
<img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='150px' height='50px' /> <br />
<h1> INVOICE </h1>  </td>
                                                    <td align='right'> <br /> <br />  
	                                                    <h3> PharmEtrade </h3>
	                                                    <h5> 36 Roremond </h5>
	                                                    <h5> WAYNE, USA </h5>
	                                                    <h5> 99887 </h5>
	                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan='2'>
                                                    <hr />
                                                    </td>
                                                    </tr>
                                                    <tr> 
	                                                    <td> 
		                                                    <b> 
			                                                    <u>BILL TO :</u> <br />
                                                            </b> 
			                                                    <span>{{CUST_NAME}}</span> <br />
			                                                    <span>{{CUST_ADDRESS1}}</span> <br />
			                                                    <span>{{CUST_ADDRESS2}}</span> <br />
			                                                    <span>{{CUST_COUNTRY}}</span> <br />
			                                                    <span>{{CUST_PINCODE}}</span> <br />
	                                                    </td> 
	                                                    <td align='right'> 
		                                                    <b> 
			                                                    <u>INVOICE NUMBER : </u><br />
		                                                    </b>
			                                                    <span>{{INVOICE_NUMBER}}</span> <br />
			                                                    <span>DATE :</span> <br />
			                                                    <span>{{INVOICE_DATE}}</span> <br />
			                                                    <span>DUE DATE :</span> <br />
			                                                    <span>{{INVOICE_DUE_DATE}}</span> <br />
	                                                    </td> 
                                                    </tr>
                                                    <tr>
                                                    <td colspan='2'>
                                                    <hr />
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan='2'>
                                                    {{PRODUCTS_DETAILS}}
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td colspan='2'>
                                                    <hr />
                                                    </td>
                                                    </tr>
                                                    <tr>
                                                    <td align='center' colspan='2'>
                                                    <p>
<img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='100px' height='25px' /> <br />
                                                    Powered by PharmETrade <br />
                                                    This invoice is a system generated and doesn't require any signature
                                                    </p>
                                                    </td>
                                                    </tr>
                                                    </table>
                                                    </td>
                                                    </tr>
                                                    </table>
                                                    </body>
                                                    </html>";
        public const string BUYER_INVOICE_TEMPLATE = @"<html>
                                                        <body style='font-family:Calibri'>
                                                        <table width='70%' align='center'>
                                                        <tr>
                                                        <td colspan='2'>
                                                        <br /> <br />
                                                        <img src='http://ec2-34-224-189-196.compute-1.amazonaws.com:5173/assets/logo2-BRJOyuYn.png' width='200px' height='45px' />
                                                        <br /> <br />
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td  colspan='2'>
                                                        {{CUST_NAME}}, <br /> <br />

                                                        Thank you for your order from PharmEtrade. Once your package ships we will send you a tracking number. You can check the status of your order by logging into your account.

                                                        If you have questions about your order, you can email us at help@pharmetrade.com.
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <br /> <br />
                                                        <h2>Your Order #{{ORDER_NUMBER}} </h2>
                                                        <h4>Placed on {{ORDER_DATE}} </h4>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <b> 
                                                            <h3>Billing Info</h3> <br />
                                                        </b> 
                                                            <span>{{CUST_NAME}}</span> <br />
                                                            <span>{{CUST_ADDRESS1}}</span> <br />
                                                            <span>{{CUST_ADDRESS2}}</span> <br />
                                                            <span>{{CUST_COUNTRY}}</span> <br />
                                                            <span>{{CUST_PINCODE}}</span> <br />
                                                        </td>
                                                        <td>
                                                        <b> 
                                                            <h3>Shipping Info</h3><br />
                                                        </b>
                                                             <span>{{CUST_NAME}}</span> <br />
                                                            <span>{{CUST_ADDRESS1}}</span> <br />
                                                            <span>{{CUST_ADDRESS2}}</span> <br />
                                                            <span>{{CUST_COUNTRY}}</span> <br />
                                                            <span>{{CUST_PINCODE}}</span> <br />
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td>
                                                        <br />
                                                        <b> 
                                                            <h2>Payment Method</h2>
                                                        </b> 
                                                            <span>{{PAYMENT_METHOD}}</span> <br />
                                                        </td>
                                                        <td>
                                                        <b> 
                                                            <h2>Shipping Method</h2>
                                                        </b>
                                                            <span>{{SHIPPING_METHOD}}</span> <br />
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan='2'>
                                                        <br />
                                                        Invoice disclaimer:
                                                        If there is a problem with your order, you have 14 days from the date of shipment to contact the seller. Some products have different policies or requirements associated with them. All returns and credits are subject to seller's approval.
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan='2'>
                                                        {{PRODUCTS_DETAILS}}
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan='2'>
                                                        <br /><br /><br /><br />
                                                        Thank you, PharmEtrade!
                                                        </td>
                                                        </tr>
                                                        </table>
                                                        </body>
                                                        </html>";
    }
}
