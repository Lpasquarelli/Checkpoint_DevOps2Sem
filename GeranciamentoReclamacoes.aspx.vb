Imports System.Data
Imports DevExpress.Web
Imports DevExpress.XtraGrid.Columns

Partial Class CadastroDeCartas
    Inherits System.Web.UI.Page
    Dim gFuncoesGerais As New FuncoesGerais
    Private Sub TransferAudit_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        ASPxWebControl.GlobalThemeBaseColor = "#2461BF"
        ASPxWebControl.GlobalTheme = "Office365"
    End Sub
    Dim eCodSolicitacao As String = ""
    Dim gCpf As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'ID_OPER.Text = "36149642801"

        If Not Page.PreviousPage Is Nothing Then
            ID_OPER.Text = CType(PreviousPage.FindControl("ID_OPER"), TextBox).Text
        Else
            If ID_OPER.Text = "" Then
                Server.Transfer(".\default.aspx")
                Exit Sub
            End If
        End If

		If hfCarregado.Value = "" Then
            hfCarregado.Value = "S"
            hfCodSessionGrid.Value = gFuncoesGerais.mGerarCodObjeto(hfIdOperador.Value & "GRID_PRINCIPAL")
			gCpf = ID_OPER.Text
			gCpf = gCpf.Remove(gCpf.Length - 2)
			mCarregarGrid(gCpf)
			preencheDadosCliente(gCpf, hfCOD_SOLICITACAO.Value)
        Else
            gFuncoesGerais.mSetGridSessionTableToGrid(grid, Session, hfCodSessionGrid.Value)
        End If

    End Sub
    Sub preencheDadosCliente(pCodEvento As String, pCodSolicitacao As String)
        Dim dtLabels As DataSet = gFuncoesGerais.mExecuteQuerySqlAzure("SELECT OPERACAO " &
                                                                            "FROM [VW_SYS_SERVDESK_COMPLETO] WHERE COD_EVENTO = '" & pCodEvento & "' AND COD_SOLICITACAO = '" & pCodSolicitacao & "' ", "servdesk")
        If dtLabels.Tables(0).Rows.Count > 0 Then


            hfOPERACAO.Value = dtLabels.Tables(0).Rows(0).Item("OPERACAO").ToString

        End If

    End Sub

    Private Sub grid_DataBound(sender As Object, e As EventArgs) Handles grid.DataBound
        Dim iGridInternal As ASPxGridView = TryCast(sender, ASPxGridView)
        If iGridInternal.Columns.IndexOf(iGridInternal.Columns("cColumnCheck")) <> -1 Then
            Return
        End If

        Dim cColumnCheck As New GridViewCommandColumn()
        cColumnCheck.Name = "cColumnCheck"
        cColumnCheck.Caption = " "
        cColumnCheck.Width = "50"
        cColumnCheck.ShowSelectCheckbox = True
        cColumnCheck.SelectAllCheckboxMode = False


        cColumnCheck.CellStyle.Paddings.PaddingTop = 4
        cColumnCheck.CellStyle.Paddings.PaddingBottom = 4
        cColumnCheck.VisibleIndex = 0

        iGridInternal.Columns.Add(cColumnCheck)
    End Sub

    Sub mCarregarGrid(pCodOperacao As String)

        gCpf = ID_OPER.Text
        gCpf = gCpf.Remove(gCpf.Length - 2)

        Dim carregaVinculoPerm As DataSet = gFuncoesGerais.mExecuteQuerySqlAzure("select OPERACAO from [PREDITIVO].[dbo].[VINCULO_PERM] (nolock) WHERE PERMISSAO LIKE '%" & gCpf & "%'", "servdesk")
        Dim operacaoTb As String = ""

        For i As Integer = 0 To carregaVinculoPerm.Tables(0).Rows().Count - 1

            operacaoTb += "'" & carregaVinculoPerm.Tables(0).Rows(i).Item("OPERACAO") & "',"

        Next



        Dim contadorOp = operacaoTb.Length() - 1
        hfOPERACAO.Value = Left(operacaoTb, contadorOp)

        Dim pWhereQuery As String = " WHERE SIT.NOME_OPERADOR = '" & pCodOperacao & "' AND STATUS_ATENDIMENTO = 'EM ANDAMENTO'"

        If lbSupervisor.Text = "S" Then pWhereQuery = ""

        Dim dtGridPrincipal As DataTable = gFuncoesGerais.mExecuteQuerySqlAzure("SELECT DISTINCT RECP.COD_RECLAMACAO " &
                                                                                   "	,RECP.DATA_CADASTRO " &
                                                                                   "	,RECP.HORA_USE_SYS AS HORA_CADASTRO " &
                                                                                   "	,RECP.DATA_USE_SYS AS DATA_RECLAMACAO " &
                                                                                   "	,RECP.DATA_PRAZO " &
                                                                                   "	,RECP.NOME_CLIENTE " &
                                                                                   "	,RECP.CPF_CLIENTE " &
                                                                                   "	,ORI.DESC_ORIGEM " &
                                                                                   "	,RECP.PROTOCOLO_RECLAMACAO" &
                                                                                   "	,CONCAT ( " &
                                                                                   "		RECP.DDD_RECLAMACAO " &
                                                                                   "		,RECP.FONE_RECLAMACAO " &
                                                                                   "		) AS TELEFONE_RECLAMACAO " &
                                                                                   "	,OBS.TEXT_OBSERVACAO AS 'OBSERVACAO' " &
                                                                                   "	,VIA.DESC_VIA AS VIA " &
                                                                                   "	,RES.DESC_RESPONSAVEL AS 'RESPONSAVEL_POSICIONAMENTO' " &
                                                                                   "    ,RECP.OPERACAO " &
                                                                                   "FROM SYS_RECLAMACAO_PRINCIPAL RECP(NOLOCK) " &
                                                                                   "LEFT JOIN SYS_RECLAMACAO_OBS OBS(NOLOCK) ON OBS.COD_RECLAMACAO = RECP.COD_RECLAMACAO " &
                                                                                   "LEFT JOIN SYS_RECLAMACAO_VIA VIA(NOLOCK) ON VIA.COD_VIA = RECP.COD_VIA " &
                                                                                   "LEFT JOIN SYS_RECLAMACAO_RESPONSAVEL RES(NOLOCK) ON RES.COD_RESPONSAVEL = RECP.COD_RESPONSAVEL " &
                                                                                   "LEFT JOIN SYS_RECLAMACAO_ORIGEM ORI(NOLOCK) ON ORI.COD_ORIGEM = RECP.COD_ORIGEM " &
                                                                                   "LEFT JOIN VW_SYS_SERVDESK_COMPLETO COM(NOLOCK) ON COM.OPERADOR_ORIGEM = RECP.COD_OPERADOR " &
                                                                                   "WHERE RECP.OPERACAO IN (" & hfOPERACAO.Value & ") AND RECP.FINALIZADO = 0", "servdesk").Tables(0)



        gFuncoesGerais.mDataTableToGridAndSession(grid, dtGridPrincipal, Session, hfCodSessionGrid.Value)
    End Sub
    Protected Sub btSairTopo_Click(sender As Object, e As ImageClickEventArgs) Handles btSairTopo.Click
        ID_OPER.Text = ""
        Page.ResolveUrl("default.aspx")
    End Sub
    Protected Sub btSobre_Click(sender As Object, e As ImageClickEventArgs) Handles btSobre.Click
        pcPopUpSobre.MinWidth = "400"
        pcPopUpSobre.MinHeight = "100"
        pcPopUpSobre.AllowDragging = False
        pcPopUpSobre.ShowOnPageLoad = True
    End Sub

    Sub mExibirMensagem(pTitulo As String, pMensagem As String)
        pcPopUpMensagens.HeaderText = pTitulo
        lbMensagemPopUp.Text = pMensagem
        pcPopUpMensagens.ShowOnPageLoad = True
    End Sub

    Protected Sub drpAplicativo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drpAplicativo.TextChanged


        Dim iCpfsSelecionados As String = ""
        Dim iNomeSelecionados As String = ""
        Dim iQtdSelecionados As Integer = 0
        Dim iDataCadSelecionados As String = ""
        Dim iHoraCadSelecionados As String = ""
        Dim iDataRecSelecionados As String = ""
        Dim iDataPrazoSelecionados As String = ""
        Dim iDescOriSelecionados As String = ""
        Dim iProtocoloSelecionados As String = ""
        Dim iTelRecSelecionados As String = ""
        Dim iObsSelecionados As String = ""
        Dim iViaSelecionados As String = ""
        Dim IRespPosSelecionados As String = ""
        Dim iOperacaoSelecionados As String = ""

        For i As Integer = 0 To grid.VisibleRowCount() - 1
            If grid.Selection.IsRowSelected(i) Then
                Dim iLinhaSelecionada As DataRowView = grid.GetRow(i)
                Dim iCodReclamacao As String = iLinhaSelecionada.Item("COD_RECLAMACAO")
                Dim iDataCad As String = iLinhaSelecionada.Item("DATA_CADASTRO")
                Dim iHoraCad As String = iLinhaSelecionada.Item("HORA_CADASTRO").ToString
                Dim iDataRec As String = iLinhaSelecionada.Item("DATA_RECLAMACAO")
                Dim iDataPrazo As String = iLinhaSelecionada.Item("DATA_PRAZO")
                Dim iNome As String = iLinhaSelecionada.Item("NOME_CLIENTE")
                Dim iCpf As String = iLinhaSelecionada.Item("CPF_CLIENTE")
                Dim iDescOri As String = iLinhaSelecionada.Item("DESC_ORIGEM")
                Dim iProtocolo As String = iLinhaSelecionada.Item("PROTOCOLO_RECLAMACAO")
                Dim iTelRec As String = iLinhaSelecionada.Item("TELEFONE_RECLAMACAO")
                Dim iObs As String = iLinhaSelecionada.Item("OBSERVACAO").ToString
                Dim iVia As String = iLinhaSelecionada.Item("VIA")
                Dim IRespPos As String = iLinhaSelecionada.Item("RESPONSAVEL_POSICIONAMENTO")
                Dim iOperacao As String = iLinhaSelecionada.Item("OPERACAO")



                iDataCadSelecionados += iDataCad & ","
                iHoraCadSelecionados += iHoraCad & ","
                iDataRecSelecionados += iDataRec & ","
                iDataPrazoSelecionados += iDataPrazo & ","
                iNomeSelecionados += iNome & ","
                iCpfsSelecionados += iCpf & ","
                iDescOriSelecionados += iDescOri & ","
                iProtocoloSelecionados += iProtocolo & ","
                iTelRecSelecionados += iTelRec & ","
                iObsSelecionados += iObs & ","
                iViaSelecionados += iVia & ","
                IRespPosSelecionados += IRespPos & ","
                iOperacaoSelecionados += iOperacao & ","


                iQtdSelecionados += 1
            End If
        Next


        ' If iIdCadastroSelecionados <> "" Then iIdCadastroSelecionados = Left(iIdCadastroSelecionados, Len(iIdCadastroSelecionados) - 1)
        If iCpfsSelecionados <> "" Then iCpfsSelecionados = Left(iCpfsSelecionados, Len(iCpfsSelecionados) - 1)

        'hfIdCadastro.Value = iIdCadastroSelecionados

        If drpAplicativo.SelectedItem.Text = "Cadastro Cliente" Then



            mExibirPopUpApp("App " & drpAplicativo.SelectedItem.Text, "AppReclamacoesCadastrarClientes.aspx?COD_USUARIO=" & ID_OPER.Text & "&CPF_CLIENTE=" & iCpfsSelecionados, "900px", "500px")


        End If

        drpAplicativo.SelectedIndex = -1
    End Sub
    Sub mExibirPopUpApp(pNameApp As String, pUrl As String, pWidth As String, pHeight As String)
        pcPopUpApps.HeaderText = pNameApp
        pcPopUpApps.ShowOnPageLoad = True
        iFrameApp.Src = pUrl
        iFrameApp.Style.Clear()
        iFrameApp.Style.Value = "height: " & pHeight & "; width: " & pWidth & ";"
    End Sub
    Protected Sub btAtualizarGrid_Click(sender As Object, e As EventArgs) Handles btAtualizarGrid.Click
        mCarregarGrid(ID_OPER.Text)
    End Sub

    Protected Sub pcPopUpApps_WindowCallback(source As Object, e As PopupWindowCallbackArgs) Handles pcPopUpApps.WindowCallback
        mCarregarGrid(ID_OPER.Text)
    End Sub

End Class


