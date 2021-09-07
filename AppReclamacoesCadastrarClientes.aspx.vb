Imports System.Data
Imports DevExpress.Web

Partial Class ClassePadrao
    Inherits System.Web.UI.Page
    Dim gFuncoesGerais As New FuncoesGerais
    Dim gcpfCliente As String = ""
    Dim gNomeCliente As String = ""
    Dim gIdCadastro As String = ""
    Dim gDataRec As String = ""
    Dim gDataPrazo As String = ""
    Dim gDescOrigem As String = ""
    Dim gProtocolo As String = ""
    Dim gTelCliente As String = ""
    Dim gObs As String = ""
    Dim gVia As String = ""
    Dim gResponsavel As String = ""


    Private Sub TransferAudit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        ASPxWebControl.GlobalThemeBaseColor = "#2461BF"
        ASPxWebControl.GlobalTheme = "Office365"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("COD_USUARIO") Is Nothing Then
            PanelPrincipal.Visible = False
            Exit Sub
        End If

        If hfCarregado.Value = "" Then
            ID_OPER.Text = Request.QueryString("COD_USUARIO")
            hfCarregado.Value = "S"

        End If

        If gIdCadastro <> "" Then
            Dim dtNomeCliente As DataSet = gFuncoesGerais.mExecuteQuerySqlAzure("SELECT RECP.COD_RECLAMACAO " &
                                                                                  "	,RECP.DATA_USE_SYS AS DATA_RECLAMACAO " &
                                                                                  "	,RECP.DATA_PRAZO " &
                                                                                  "	,RECP.NOME_CLIENTE " &
                                                                                  "	,RECP.CPF_CLIENTE " &
                                                                                  "	,ORI.DESC_ORIGEM " &
                                                                                  "	,RECP.PROTOCOLO_RECLAMACAO AS PROTOCOLO " &
                                                                                  "	,CONCAT ( " &
                                                                                  "		RECP.DDD_RECLAMACAO " &
                                                                                  "		,RECP.FONE_RECLAMACAO " &
                                                                                  "		) AS TELEFONE_RECLAMACAO " &
                                                                                  "	,OBS.TEXT_OBSERVACAO AS 'OBSERVACAO' " &
                                                                                  "	,VIA.DESC_VIA AS VIA " &
                                                                                  "	,RES.DESC_RESPONSAVEL AS 'RESPONSAVEL_POSICIONAMENTO' " &
                                                                                  "FROM SYS_RECLAMACAO_PRINCIPAL RECP(NOLOCK) " &
                                                                                  "LEFT JOIN SYS_RECLAMACAO_OBS OBS(NOLOCK) ON OBS.COD_RECLAMACAO = RECP.COD_RECLAMACAO " &
                                                                                  "LEFT JOIN SYS_RECLAMACAO_VIA VIA(NOLOCK) ON VIA.COD_VIA = RECP.COD_VIA " &
                                                                                  "LEFT JOIN SYS_RECLAMACAO_RESPONSAVEL RES(NOLOCK) ON RES.COD_RESPONSAVEL = RECP.COD_RESPONSAVEL " &
                                                                                  "LEFT JOIN SYS_RECLAMACAO_ORIGEM ORI(NOLOCK) ON ORI.COD_ORIGEM = RECP.COD_ORIGEM " &
                                                                                  "LEFT JOIN VW_SYS_SERVDESK_COMPLETO COM(NOLOCK) ON COM.OPERADOR_ORIGEM = RECP.COD_OPERADOR " &
                                                                                  "WHERE RECP.CPF_CLIENTE = '" & gIdCadastro & "' ", "servdesk")

            If dtNomeCliente.Tables(0).Rows.Count > 0 Then
                gNomeCliente = dtNomeCliente.Tables(0).Rows(0).Item("NOME_CLIENTE").ToString

            Else
                gNomeCliente = ""
            End If

            gDataRec = Request.QueryString("DATA_RECLAMACAO")
            gDataPrazo = Request.QueryString("DATA_PRAZO")
            gDescOrigem = Request.QueryString("DESC_ORIGEM")
            gProtocolo = Request.QueryString("PROTOCOLO")
            gTelCliente = Request.QueryString("TELEFONE_RECLAMACAO")
            gObs = Request.QueryString("OBSERVACAO")
            gVia = Request.QueryString("VIA")
            gResponsavel = Request.QueryString("RESPONSAVEL_POSICIONAMENTO")
            gcpfCliente = Request.QueryString("CPF_CLIENTE")

            txtNomeCliente.Text = gNomeCliente
            txtCPFCliente.Text = gcpfCliente
            dtDataReclamacao.Text = gDataRec
            dtPrazo.Text = gDataPrazo
            ddlOrigemReclamacao.SelectedIndex.Equals(gDescOrigem)
            txtProtocolo.Text = gProtocolo
            txtTelefone.Text = gTelCliente
            ddlVia.SelectedIndex.Equals(gVia)
            ddlResponsavel.SelectedIndex.Equals(gResponsavel)
            txtObs.Text = gObs

        End If

        ddlOperacao.Items.Clear()

        If ddlOperacao.Items.Count = 0 Then
            dadosOperacao()
        End If



    End Sub

    Public Sub MsgAjax(ByVal msg As String)

        Dim myScript As String = "alert('" + msg + "');"

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "UserSecurity", myScript, True)

    End Sub

    Private Sub dadosOperacao()
        Dim gOperacao As DataSet
        Dim gCpf As String = ID_OPER.Text
        gCpf = gCpf.Remove(gCpf.Length - 2)
        gOperacao = gFuncoesGerais.mExecuteQuerySqlAzure("select OPERACAO from [PREDITIVO].[dbo].[VINCULO_PERM] (nolock) WHERE PERMISSAO LIKE '%" & gCpf & "%' AND OPERACAO NOT IN('SUPORTE'', 'TESTE', 'TI')", "servdesk")

        For i As Integer = 0 To gOperacao.Tables(0).Rows().Count - 1
            ddlOperacao.Items.Add(gOperacao.Tables(0).Rows(i).Item("OPERACAO"))
        Next
    End Sub


    Protected Sub btCadastrar_Click(sender As Object, e As EventArgs) Handles btCadastrar.Click
        If gIdCadastro = "" Then
            insereClienteNovo()
        End If

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "FechaModal", "parent.mCloseModalPopUpApp()", True)
    End Sub

    Sub insereClienteNovo()



        txtNomeCliente.Text = txtNomeCliente.Text.Replace("-", "").Replace(".", "").Replace("'", "").Trim()
        txtCPFCliente.Text = txtCPFCliente.Text.Replace(" ", "").Replace("-", "").Replace(".", "").Trim()
        txtTelefone.Text = txtTelefone.Text.Replace("(", "").Replace(")", "").Replace("-", "").Trim()
        Dim ddd As String = Mid(txtTelefone.Text, 1, 2)
        Dim foneCli As String = Mid(txtTelefone.Text, 3, 11)
        Dim codOrigem As String = ddlOrigemReclamacao.SelectedItem.Value
        Dim codVia As String = ddlVia.SelectedItem.Value
        Dim codResponsavel As String = ddlResponsavel.SelectedItem.Value

        If txtNomeCliente.Text = "" Then

            MsgAjax("Insira um Nome válido.")
            Exit Sub

        ElseIf txtCPFCliente.Text = "" Or IsNumeric(txtCPFCliente.Text) = False Or Len(txtCPFCliente.Text) <> 11 Then

            MsgAjax("Insira um CPF válido.")
            Exit Sub

        ElseIf gFuncoesGerais.verificaCPF(txtCPFCliente.Text) = False Then

            MsgAjax("Insira um CPF válido.")
            Exit Sub

        ElseIf (txtObs.Text <> "") Then
            Dim dtInsereMain = gFuncoesGerais.mExecuteQuerySqlAzure("INSERT INTO SYS_RECLAMACAO_PRINCIPAL( " &
                                                                       "NOME_CLIENTE, " &
                                                                       "CPF_CLIENTE, " &
                                                                       "DATA_CADASTRO, " &
                                                                       "DATA_PRAZO, " &
                                                                       "DATA_USE_SYS, " &
                                                                       "HORA_USE_SYS, " &
                                                                       "PROTOCOLO_RECLAMACAO, " &
                                                                       "DDD_RECLAMACAO, " &
                                                                       "FONE_RECLAMACAO, " &
                                                                       "COD_OPERADOR, " &
                                                                       "COD_ORIGEM, " &
                                                                       "COD_VIA, " &
                                                                       "OPERACAO," &
                                                                       "COD_RESPONSAVEL, FINALIZADO) output inserted.COD_RECLAMACAO VALUES ( " &
                                                                       "'" & txtNomeCliente.Text & "', " &
                                                                       "'" & txtCPFCliente.Text & "', " &
                                                                       "(select CONVERT(DATE, SWITCHOFFSET(getdate(), DATEPART(TZOFFSET, getdate() AT TIME ZONE 'E. South America Standard Time')))), " &
                                                                       "'" & dtPrazo.Text & "', " &
                                                                       "'" & dtDataReclamacao.Text & "', " &
                                                                       "(select CONVERT(TIME(0), SWITCHOFFSET(getdate(), DatePart(TZOFFSET, getdate() AT TIME ZONE 'E. South America Standard Time')))), " &
                                                                       "'" & txtProtocolo.Text & "', " &
                                                                       "'" & ddd & "', " &
                                                                       "'" & foneCli & "', " &
                                                                       "'" & ID_OPER.Text & "', " &
                                                                       "'" & codOrigem & "', " &
                                                                       "'" & codVia & "', " &
                                                                       "'" & ddlOperacao.Text & "', " &
                                                                       "'" & codResponsavel & "', 0)", "servdesk")

            Dim codReclamacao As String = dtInsereMain.Tables(0).Rows(0).Item("COD_RECLAMACAO").ToString


            dtInsereMain = gFuncoesGerais.mExecuteQuerySqlAzure("INSERT INTO SYS_RECLAMACAO_OBS (" &
                                                                  "COD_RECLAMACAO, " &
                                                                  "TEXT_OBSERVACAO)" &
                                                                  "VALUES " &
                                                                  "('" & codReclamacao & "', " &
                                                                  "'" & txtObs.Text & "' " &
                                                                  ")", "servdesk")




        Else
            Dim dtInsereMain = gFuncoesGerais.mExecuteQuerySqlAzure("INSERT INTO SYS_RECLAMACAO_PRINCIPAL( " &
                                                                       "NOME_CLIENTE, " &
                                                                       "CPF_CLIENTE, " &
                                                                       "DATA_CADASTRO, " &
                                                                       "DATA_PRAZO, " &
                                                                       "DATA_USE_SYS, " &
                                                                       "HORA_USE_SYS, " &
                                                                       "PROTOCOLO_RECLAMACAO, " &
                                                                       "DDD_RECLAMACAO, " &
                                                                       "FONE_RECLAMACAO, " &
                                                                       "COD_OPERADOR, " &
                                                                       "COD_ORIGEM, " &
                                                                       "COD_VIA, " &
                                                                       "COD_RESPONSAVEL, FINALIZADO) VALUES ('" & txtNomeCliente.Text & "', " &
                                                                       "'" & txtCPFCliente.Text & "', " &
                                                                       "(select CONVERT(DATE, SWITCHOFFSET(getdate(), DATEPART(TZOFFSET, getdate() AT TIME ZONE 'E. South America Standard Time')))), " &
                                                                       "'" & dtPrazo.Text & "', " &
                                                                       "'" & dtDataReclamacao.Text & "', " &
                                                                       "(select CONVERT(TIME(0), SWITCHOFFSET(getdate(), DatePart(TZOFFSET, getdate() AT TIME ZONE 'E. South America Standard Time')))), " &
                                                                       "'" & txtProtocolo.Text & "', " &
                                                                       "'" & ddd & "', " &
                                                                       "'" & foneCli & "', " &
                                                                       "'" & ID_OPER.Text & "', " &
                                                                       "'" & codOrigem & "', " &
                                                                       "'" & codVia & "', " &
                                                                       "'" & codResponsavel & "', 0)", "servdesk")
        End If


    End Sub

End Class


