<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AppReclamacoesCadastrarClientes.aspx.vb" Inherits="ClassePadrao" %>

<%@ Register Assembly="DevExpress.Web.v18.1, Version=18.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Servcredi App Cadastrar Clientes</title>
    <style type="text/css">
        .centerTextBox input {
            text-align: center;
        }
        .auto-style1 {
            height: 32px;
        }
        .auto-style2 {
            height: 40px;
        }
        .auto-style5 {
            width: 402px;
        }
        .auto-style6 {
            height: 40px;
            width: 402px;
        }
        .auto-style7 {
            width: 344px;
        }
        .auto-style8 {
            height: 40px;
            width: 344px;
        }
    </style>
</head>
<body style="font-family: Calibri; margin-left: 0px; margin-right: 0px; overflow-y: auto; overflow-x: auto;">
    <form id="form1" runat="server">

        <script type="text/javascript">
            Sys.Application.add_load(ApplicationLoadHandler);
            function ApplicationLoadHandler(sender, args) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
                if (!prm.get_isInAsyncPostBack()) {
                    prm.add_initializeRequest(initRequest);
                    prm.add_endRequest(endRequest);
                }
            }
 
             
            function initRequest() {
                mShowLoading();
            }
            function mShowLoading() {
                //parent.mMostrarCarregandoExterno();
            }
            function endRequest() {
                //parent.mOcultarCarregandoExterno();
            }            
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="PanelPrincipal">
                    <center>
                        <table>
                            <tr>
                                <td class="auto-style7">
                                    <dx:ASPxTextBox runat="server" ID="txtNomeCliente" Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" CssClass="centerTextBox" Height="32px" width="300px" NullText="Nome (Digite aqui)..."  MaxLength="100" Theme="iOS">
                                        <Border BorderStyle="None" />
                                    </dx:ASPxTextBox>
                                </td>
                                
                                <td>
                                    <dx:ASPxTextBox runat="server" ID="txtCPFCliente" CssClass="centerTextBox" Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Height="32px" NullText="CPF (Digite aqui)..." MaxLength="11" Theme="iOS">
                                          <MaskSettings Mask="000.000.000-00" IncludeLiterals="None" />
                                         <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ErrorTextPosition="Bottom" />
                                        <Border BorderStyle="None" />
                                    </dx:ASPxTextBox>
                                </td>
                            </tr>

                            <tr align="left">
                                <td class="auto-style7">
                                 <dx:ASPxDateEdit ID="dtDataReclamacao" runat="server"  Width="150px" Caption="Data Reclamação" Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;"  DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd" Theme="iOS">
                                         <CalendarProperties>
                                             <FastNavProperties DisplayMode="Inline" />
                                         </CalendarProperties>
                                     </dx:ASPxDateEdit>
                                </td>
                                
                                    <td>
                                      <dx:ASPxDateEdit ID="dtPrazo" runat="server" EditFormat="Custom"  Width="150px" Caption="Data prazo" Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;"  DisplayFormatString="yyyy-MM-dd" EditFormatString="yyyy-MM-dd" Theme="iOS">
                                         <CalendarProperties>
                                             <FastNavProperties DisplayMode="Inline" />
                                         </CalendarProperties>
                                     </dx:ASPxDateEdit>
                                </td>
                            </tr>
                             <tr>
                            <td class="auto-style7">&nbsp;</td>
                            </tr>
                             <tr align="center">
                                
                                 <td class="auto-style8" align="left">
                                   <dx:ASPxComboBox runat="server" ID="ddlOrigemReclamacao" ClientInstanceName="ddlOrigemReclamacao"  Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Caption="Origem Reclamação" Height="32px" DropDownRows="10" AutoPostBack="true" Theme="iOS" Width="150px">
                                        <Items>
                                            <dx:ListEditItem Text="Selecione" Value="0" />
                                            <dx:ListEditItem Text="CENTRAL" Value="1" />
                                            <dx:ListEditItem Text="BACEN" Value="2" />
                                            <dx:ListEditItem Text="SAC INTERNO" Value="3" />
                                            <dx:ListEditItem Text="﻿SAC EXETERNO" Value="4" />
                                            <dx:ListEditItem Text="CONSUMIDOR" Value="5" />
                                            <dx:ListEditItem Text="RECLAME AQUI" Value="6" />
                                            <dx:ListEditItem Text="JURIDICO" Value="7" />
                                            <dx:ListEditItem Text="PROCON" Value="8" />
                                            <dx:ListEditItem Text="RECEPÇÃO" Value="9" />
                                            <dx:ListEditItem Text="OUVIDORIA" Value="10" />
                                            <dx:ListEditItem Text="FACEBOOK" Value="11" />
                                        </Items>


                                    </dx:ASPxComboBox>                               
                                </td>
                                    <td align="center" class="auto-style2">
                                    <dx:ASPxTextBox runat="server" ID="txtProtocolo" CssClass="centerTextBox" Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Height="32px" NullText=" N° Protocolo (Digite aqui)...">
                                        <Border BorderStyle="None" />
                                    </dx:ASPxTextBox>
                                </td>
                                 
                            </tr>
                            <tr>
                            <td class="auto-style7">&nbsp;</td>
                            </tr>

                       
                          <tr>
                              <td>
                                   <dx:ASPxComboBox runat="server" ID="ddlResponsavel" ClientInstanceName="ddlResponsavel"  Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Caption="Responsavel Origem" Height="32px" DropDownRows="10" AutoPostBack="true" Theme="iOS" Width="203px">
                                        <Items>
                                            <dx:ListEditItem Text="Selecione..." Value="0" />
                                            <dx:ListEditItem Text="DIMDIM" Value="1" />
                                            <dx:ListEditItem Text="BANCO" Value="2" />
                                        </Items>
                                    </dx:ASPxComboBox>  
                                </td>
                              <td>
                               <dx:ASPxComboBox runat="server" ID="ddlVia" ClientInstanceName="ddlVia"  Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Caption="VIA" Height="32px" DropDownRows="10" AutoPostBack="true" Theme="iOS">
                                        <Items>
                                            <dx:ListEditItem Text="Selecione..." Value="0" />
                                            <dx:ListEditItem Text="CORREIOS" Value="1" />
                                            <dx:ListEditItem Text="DIGITAL" Value="2" />
                                            <dx:ListEditItem Text="EMAIL_CORREIO" Value="3" />
                                            <dx:ListEditItem Text="MOTOBOY" Value="4" />
                                            <dx:ListEditItem Text="SMS" Value="5" />
                                        </Items>
                                   </dx:ASPxComboBox>  
                                   </td>

                                    
                            </tr>
           
                        </table>
                        <table>
                                <tr>
                               
                                     <td align="center" class="auto-style8">
                                        <dx:ASPxTextBox ID="txtTelefone" runat="server" align="center" Width="68%" ClientInstanceName="clTxtPhone" Caption="N° Telefone" >
                                        <MaskSettings Mask="(99)00000-0000" IncludeLiterals="None" />
                                         <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ErrorTextPosition="Bottom" />
                                        
                                        <Border BorderStyle="None" />
                                       </dx:ASPxTextBox>
                                       </td>

                           

                            </tr>
                             <tr align="center">
                                
                                 <td class="auto-style8" align="left">
                                   <dx:ASPxComboBox runat="server" ID="ddlOperacao" ClientInstanceName="ddlOrigemReclamacao"  Style="border-bottom: 1px solid; border-bottom-color: #c7c7c7;" Caption="Operação" Height="32px" DropDownRows="10" AutoPostBack="true" Theme="iOS" Width="236px">
                                         <Items>
                                            <dx:ListEditItem Text="Selecione..." Value="0" />
                                     
                                        </Items>
                                    </dx:ASPxComboBox>            
                            </tr>
                        </table>
                       
                  <asp:TextBox ID="txtObs" Font-Names="Consolas" placeholder="Escreva aqui seu texto com até no máximo 500 caracteres" runat="server" Height="110px" Width="505px" TextMode="MultiLine" Visible="True" MaxLength="500" Rows="100" ></asp:TextBox>

                        <br />
                        <br />
                        <table>
                         
                            <tr>
                                <td align="center">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="auto-style1">
                                    <dx:ASPxButton runat="server" ID="btCadastrar" Text="Cadastrar" Style="border: 1px solid #c7c7c7; background-color: white; color: #666666; "></dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </center>
                    </asp:Panel>
                <asp:Panel runat="server">
                    <asp:HiddenField ID="hfCarregado" runat="server" />
                </asp:Panel>
                <asp:ListBox runat="server" ID="listOrgao" Visible="false"></asp:ListBox>
                <asp:ListBox runat="server" ID="listEmpregador" Visible="false"></asp:ListBox>
                <asp:TextBox ID="ID_OPER" runat="server" Visible="false"></asp:TextBox>
				
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
