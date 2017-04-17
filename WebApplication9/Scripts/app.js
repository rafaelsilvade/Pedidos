$(function () {
    $('.listSelectfind').prepend('<option selected="selected" value="0">Selecione</option>');

    var cli = getParam(window.location.href, "clienteid");
    var numero = getParam(window.location.href, "numero");
    var eini = getParam(window.location.href, "ini");
    var efim = getParam(window.location.href, "fim");
    if (cli != null)
        $('.listSelectfind option').each(function (index, element) {
            if ($(this).attr("value") == cli) {
                $('.listSelectfind option[selected="selected"]').removeAttr("selected");
                $(this).attr("selected", "selected");
             }
            
        }
        );
    if (numero != null)
        $("#numero").attr("value", numero);
    if (eini != null)
        $("#ini").attr("value", eini);
    if (efim != null)
        $("#fim").attr("value", efim);

    $('.listSelectfind').select2({});
    $('.datatable_pre').DataTable({
        'paging': false,
        'columnDefs': [
            {
                "targets": [$('.datatable_pre thead tr th').size() - 1],
                "visible": true,
                "searchable": false,
                "orderable": false
            }
        ]


    });
    $('.datepicker').datepicker({ format: "yyyy-mm-dd" });
    $('#groupPanel').hide();
   

    $('.pesquisa').click(function (e) {
        var url = "";    
        if ($(this).attr("href") != '#')
            url = $(this).attr("href");
        url += "?";
        if ($("#ClienteID").val() > 0)
            url += "clienteid=" + $("#ClienteID").val()+"&";
        if ($("#numero").val() != "")
            url += "numero=" + $("#numero").val() + "&";
        if ($("#ini").val() != "" && $("#fim").val() != "")
            if ($("#fim").val() >= $("#ini").val())
            url += "ini=" + $("#ini").val() + "&fim="+$("#fim").val();
            else
                alert("A data final tem que ser maior que a data inicial.")
        document.location = url;


        e.preventDefault();
    });
})
function getParam(url, param) {
    if (typeof url != 'string' || typeof param != 'string') {
        return;
    }
    var regex = new RegExp('[\\?&]' + param + '=' + '(.+?)([&#]|$)', 'i');
    return (value = (regex.exec(window.location.href) || [, null])[1])
        ? decodeURIComponent(value)
        : null;
}
function ListarItens(idPedido) {

    var url = "/itensPedidoes/List/"+idPedido;


    $.get(url, function (data) {
        $('.datatable_pre tbody').replaceWith($('tbody', data));
    });

}


function SalvarItens() {

    var ProdutoID = $("#ProdutoID").val();
    var qtde = $("#qtde").val();
    var PedidoID = $("#PedidoID").val();

    var url = "/itensPedidoes/Create";

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , data: { ProdutoID: ProdutoID, qtde: qtde, PedidoID: PedidoID }
        , success: function (data) {
            if (data.Resultado > 0) {
                ListarItens(data.Resultado);
                $('#totalvl').html('Valor total: ' + data.ValorTotal);
            }
        }
    });
}


function SalvarPedido() {
    var ClienteID = $("#ClienteID").val();
    var dataEntrega = $("#dataEntrega").val();

    var token = $('input[name="__RequestVerificationToken"]').val();
    var tokenadr = $('form[action="/Pedidoes/Create"] input[name="__RequestVerificationToken"]').val();
    var headers = {};
    var headersadr = {};
    headers['__RequestVerificationToken'] = token;
    headersadr['__RequestVerificationToken'] = tokenadr;

    var url = "/Pedidoes/Create";

    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , headers: headersadr
        , data: { dataEntrega: dataEntrega, ClienteID: ClienteID, __RequestVerificationToken: token }
        , success: function (data) {
            if (data.Resultado > 0) {
                ListarItens(data.Resultado);
                $('#PedidoID').attr('value', (data.Resultado));
                $("#ClienteID").prop('disabled', true);
                $("#dataEntrega").prop('disabled', true);
                $('.btn-success').hide();
                $('#groupPanel').show();
                
            }
        }
    });
}

function deletaitempedido(pedidoID, produtoID) {

    var url = "/ItensPedidoes/Delete/" + pedidoID + "/" + produtoID;
    $.ajax({
        url: url
        , type: "POST"
        , datatype: "json"
        , success: function (data) {
            if (data.Resultado > 0) {
                ListarItens(data.Resultado);
                $('#totalvl').html('Valor total: ' + data.ValorTotal);
            }
        }
    });
}