// V a r i a b l e s    p u b l i c a s

// PRD

//var url_api_logs = 'https://monitorapi-dtv.ocasa.com/api/ListErrors';
//var url_api_logtype = 'https://monitorapi-dtv.ocasa.com/api/ListIntegracion';
//var url_api_logs_detail = 'https://monitorapi-dtv.ocasa.com/api/ListLogDetail';
//var url_api_apps = 'https://monitorapi-dtv.ocasa.com/api/ListStates';
//var url_api_update_state = 'https://monitorapi-dtv.ocasa.com/api/UpdateLogState';
//var url_api_responsable = 'https://monitorapi-dtv.ocasa.com/api/ListResponsable';
//var url_api_responsable = 'https://monitorapi-dtv.ocasa.com/api/ListResponsable';
//var url_api_savechanges = 'https://monitorapi-dtv.ocasa.com/api/SaveChanges';
////var url_api_savedocument = 'https://localhost:44354/AsnTeorico/GuardarArchivo';
//var url_api_savedocument = 'https://integracionocasadtv2.ocasa.com/AsnTeorico/GuardarArchivo';


//DEVELOP
var url_api_logs = 'https://localhost:44361/api/ListErrors';
var url_api_logtype = 'https://localhost:44361/api/ListIntegracion';
var url_api_logs_detail = 'https://localhost:44361/api/ListLogDetail';
var url_api_apps = 'https://localhost:44361/api/ListStates';
var url_api_update_state = 'https://localhost:44361/api/UpdateLogState';
var url_api_responsable = 'https://localhost:44361/api/ListResponsable';
var url_api_savechanges = 'https://localhost:44361/api/SaveChanges';
var url_api_savedocument = 'https://localhost:44354/AsnTeorico/GuardarArchivo'

//Qas

//var url_api_logs = 'https://directvqas.ocasa.com/MonitorDtvApi/api/ListErrors';
//var url_api_logtype = 'https://directvqas.ocasa.com/MonitorDtvApi/api/ListIntegracion';
//var url_api_logs_detail = 'https://directvqas.ocasa.com/MonitorDtvApi/api/ListLogDetail';
//var url_api_apps = 'https://directvqas.ocasa.com/MonitorDtvApi/api/ListStates';
//var url_api_update_state = 'https://directvqas.ocasa.com/MonitorDtvApi/api/UpdateLogState';
//var url_api_responsable = 'https://directvqas.ocasa.com/MonitorDtvApi/api/ListResponsable';
//var url_api_savechanges = 'https://directvqas.ocasa.com/MonitorDtvApi/api/SaveChanges';
//var url_api_savedocument = 'https://directvqas.ocasa.com/Integracion-Dtv/AsnTeorico/GuardarArchivo';
//var url_api_savedocument = 'https://localhost:44354//GuardarArchivo';

var today = new Date();

let yyyy = today.getFullYear();
let MM = today.getMonth() + 1; // Months start at 0!
let dd = today.getDate();
let hh = today.getHours();
let mm = today.getMinutes();
let ss = today.getSeconds();

let formattedDateSQL = yyyy + '-' + MM + '-' + dd;
let formattedDateScreen = pad(2, dd, '0') + '/' + pad(2, MM, '0') + '/' + yyyy;
let formattedTimeScreen = pad(2, hh, '0') + ':' + pad(2, mm, '0') + ':' + pad(2, ss, '0');
let type_option = 0;
let id_application = 0;
let refresh_interval = 10;  // En segundos
let refresh_clock_interval = 1; // En segundos

let interval_legend = '';

let idLogInterval = 0;
let idClockInterval = 0;

const records = [];

let estados;
let responsables;

SetDateToSearch(dd+'/'+mm+'/'+yyyy);

$(document).ready(function () {
    InitializeControls();

    $('#datepickerDesde').datepicker({
        locale: 'es',
        autoclose: true,
        todayHighlight: true,
        autoclose: true,
        endDate: '+1d',
        datesDisabled: '+1d',
        orientation: "top",
        format: 'dd/mm/yyyy',
        keyboardNavigation: true
    });
    $('#datepickerHasta').datepicker({
        locale: 'es',
        autoclose: true,
        todayHighlight: true,
        autoclose: true,
        endDate: '+1d',
        datesDisabled: '+1d',
        orientation: "top",
        format: 'dd/mm/yyyy',
        keyboardNavigation: true
    });
    var cargando = $("#cargando");
    $(document).ajaxStart(function () {
        cargando.show();
    });
    $(document).ajaxStop(function () {
        cargando.hide();
    });
});

// F U N C I O N E S 

function InitializeControls() {

    LoadIntegracion();
    LoadEstados();
    LoadResponsable();
    LoadErrores('0');

    //style.disabled = true;
   // Setea el intervalo para la consulta del log
   //idLogInterval=setInterval(LoadLogs, refresh_interval * 1000);
   // Setea el intervalo para la consulta del reloj
   //idClockInterval=setInterval(ShowClock, refresh_clock_interval * 1000);
   //interval_legend = Get_legend_interval(refresh_interval);
   //$('#intervalo').html(interval_legend);

   $('#buscar').click(function () {      
       LoadErrores("1");
   });

   $('#combo_app').change(function () {
       //LoadLogs();
       LoadErrores(1);
   });
   $('#combo_paises').change(function () {
       LoadErrores(1);
   });
    $('#combo_integracion').change(function () {
        LoadErrores(1);
    });
}

function LoadErrores(init) {
    let html = '';
    let nroDoc = '';
    let estado = '';
    let fechaDesde = '';
    let fechaHasta = '';
    let pais = '';
    let integracion = '';

    if (init == 0) {
        nroDoc = '0';
        estado = '0';
        fechaDesde = '';
        fechaHasta = '';
        pais = '';
        integracion = '0';
    }
    else {
        nroDoc = $('#nroDoc').val();
        pais = $('#combo_paises').val();
        estado = $('#combo_app').val();
        integracion = $('#combo_integracion').val();

        let formatearFecha = $('#datepickerDesde').val().split('/');

        if (formatearFecha != '') {
            let year = formatearFecha[2];
            //let month = formatearFecha[1] - 1;
            let month = formatearFecha[1];
            let day = formatearFecha[0];
            fechaDesde = day + '-' + month + '-' + year;
            formatearFecha = $('#datepickerHasta').val().split('/')
            year = formatearFecha[2];
            //month = formatearFecha[1] - 1;
            month = formatearFecha[1];
            day = formatearFecha[0];
            fechaHasta = day + '-' + month + '-' + year;

            if (new Date(fechaDesde) > new Date(fechaHasta)) {
                alert("Fecha Desde no puede ser mayor a Fecha Hasta");
                $('#datepickerDesde').val('');
                $('#datepickerHasta').val('');
            }
        }
    }

    html = GetHtmlSpinner();

    $('#data').html(html);
    $('#message').html('<span class="text-primary">Consultando logs del ' + formattedDateScreen + '</span>');

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: url_api_logs,
        data: JSON.stringify({
          'nroDoc': nroDoc,
          'estado': estado,
          'fechaDesde': fechaDesde,
          'fechaHasta': fechaHasta,
          'pais': pais,
          'integracion': integracion
      }),
      dataType: "json"
   }).done(function (data) {

       $('#data').append('');

       if (data != "") {

           let index = 0;
           let records_count = 0;
           let html_row = '';
           let color = '';
           let Estado = '';
           let IdDocumento = '';
           let FechaError = 0;
           let NombreArchivo = '';
           let Integracion = '';
           let IdPais = '';
           //let Estado = '';
           let Responsable = '';
           let FechaCierre = '';
           let error = '';
           let Observacion = '';
           let actual_time = new Date();
           let yyyy = actual_time.getFullYear();
           let MM = actual_time.getMonth() + 1; // Months start at 0!
           let dd = actual_time.getDate();
           let hh = actual_time.getHours();
           let mm = actual_time.getMinutes();
           let ss = actual_time.getSeconds();
           let formattedTimeScreen = pad(2, hh, '0') + ':' + pad(2, mm, '0') + ':' + pad(2, ss, '0');
           let date = $('#fecha').val();
           let datesql = '';
           // Validaciones de parametros
           if (date === '' || date === undefined)
               datesql = formattedDateSQL;
           else {
               let [day, month, year] = date.split('/')
               datesql = year + '-' + month + '-' + day;
           }

           $('#data').empty();

           if (data.items != null) {

               html_row = '<br/>';
               html_row += '<div class="row xxsmall" style="border:1px solid; border-color:#BBB2B0; padding-bottom:2px;padding-top:2px;">';
               html_row += '     <div class="col-lg-12"><span class="text-bold">Actualizado a las ' + formattedTimeScreen +'</span></div>';
               html_row += '<table class="table table-editable" style="border:1px solid; border-color:#BBB2B0; padding-bottom:2px;padding-top:2px;" >';
               html_row += '<thead>';
               html_row += '<tr>';
               html_row += '     <th scope="col">Nro Documento</span></th>';
               html_row += '     <th scope="col">Fecha de Error</span></th>';
               html_row += '     <th scope="col">Nombre de Archivo</span></th>';
               html_row += '     <th scope="col">Integracion</span></th>';
               html_row += '     <th scope="col">Pais</span></th>';
               html_row += '     <th scope="col">Error</span></th>';
               html_row += '     <th scope="col">Estado</span></th>';
               html_row += '     <th scope="col">Responsable</span></th>';
               html_row += '     <th scope="col">Fecha de Cierre</span></th>';
               html_row += '     <th scope="col">Observacion</span></th>';
               html_row += '     <th scope="col" style="padding-left: 25px; padding-right: 25px">Acciones</span></th>';
               html_row += '</tr></thead>';
           }
           if (data.items != null) {
               records_count = data.items.length;
           }
           if (records_count > 0) {
               while (index < data.items.length) {
                   records_count++;
                   Clave = data.items[index].Clave;
                   Fecha_sys = data.items[index].Fecha_sys;
                   Fecha_Vcia = data.items[index].Fecha_Vcia;
                   Usuario = data.items[index].Usuario;
                   Desc_Corta = data.items[index].Desc_Corta;
                   Desc_Larga = data.items[index].Desc_Larga;
                   Estado = data.items[index].Estado;
                   IdDocumento = data.items[index].IdDocumento;
                   FechaError = data.items[index].FechaError;
                   NombreArchivo = data.items[index].NombreArchivo;
                   Integracion = data.items[index].DescripIntegra;
                   IdPais = data.items[index].IdPais;
                   Responsable = data.items[index].DescripResponsa;
                   FechaCierre = data.items[index].FechaCierre;
                   error = data.items[index].Error;
                   Observacion = data.items[index].Observacion;

                   color = 'text-info';

               //if (level === 1) color = 'text-info';
               //if (level === 2) color = 'text-warning';
               //if (level === 3) color = 'text-danger';

                   html_row += '<t<body>';
                   html_row += '<tr>';

                   html_row += '<td>'
                   html_row += '     <span style="cursor:pointer;">' + IdDocumento + '<span>';
                   html_row += '</td>';

                   html_row += '<td>'
                   html_row += '     <span>' + FechaError + '<span>';
                   html_row += '</td>';

                   html_row += '<td>'
                   html_row += NombreArchivo;
                   html_row += '</td>';

                   html_row += '<td>'
                   html_row += '     <span>' + Integracion + '<span>';
                   html_row += '</td>';

                   html_row += '<td>'
                   html_row += '     <span>' + IdPais + '<span>';
                   html_row += '</td>';

                   html_row += '<td>'
                   html_row +=  error;
                   html_row += '</td>';

                   html_row += '<td>';
                   html_row += '<select class="form-control form-control-sm estado" iddocu="' + IdDocumento+'" onchange="onChange(this)">';
                   for (var i = 0; i < estados.length; i++) {

                       if (Estado == estados[i].IdEstado) {
                           html_row += '  <option selected value="' + estados[i].Clave + '">' + estados[i].DescripEstado + '</option>';
                       }
                       else {
                           html_row += '  <option value="' + estados[i].Clave + '">' + estados[i].DescripEstado + '</option>';
                       }
                   }
                   html_row += '</select>';
                   html_row += '</td>';
                   html_row += '<td>'
                   html_row += '<select class="form-control form-control-sm responsable" iddocu="' + IdDocumento +'" onchange="onChange(this)">';
                
                for (let i = 0; i < responsables.length; i++) {
                    if (Responsable == responsables[i].DescripResponsa) {
                        html_row += '  <option selected value="' + responsables[i].Clave + '">' + responsables[i].DescripResponsa + '</option>';
                    }
                    else {
                        html_row += '  <option value="' + responsables[i].Clave + '">' + responsables[i].DescripResponsa + '</option>';
                    }
                }

                html_row += '</select>';
                html_row += '</td>';

                html_row += '<td>'
                html_row += '     <span>' + FechaCierre + '<span>';
                html_row += '</td>';

                html_row += '<td cursor:pointer;" id="obs' + IdDocumento + '" detail="' + Observacion + '" onkeydown="EnableButton(' + IdDocumento + ')">'
                //html_row += Observacion;
                html_row += '<a href="#ex1" rel="modal:open" type="button" onClick="ShowObservacion( ' + IdDocumento + ')"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-search manual-ajax observacion" viewBox="0 0 16 16" disabled>'
                html_row += '<path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001q.044.06.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1 1 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0"/></svg></a>'
                html_row += '</td>';
                //onclick = "ShowLogsDetails(' + Clave + ',\'' + Usuario + '\',\'' + Fecha_Vcia + '\',' + IdDocumento + '\');
                html_row += '<td>'

                html_row += '<button class"svg-button" style="background-color: white; border:0" onclick="GuardarCambio(' + IdDocumento + ')" id="edit' + IdDocumento + '" disabled>'
                html_row += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">'
                html_row += '<path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>'
                html_row += '<path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>'
                html_row += '</svg>'
                html_row += '</button>'
                //html_row += '<button type="button" class="btn btn-light btn-rounded btn-sm my-0" onclick="GuardarCambio(' + IdDocumento + ')"  id="edit' + IdDocumento + '" disabled>'
                //html_row += 'Editar </button>'
                html_row += '<button class"svg-button" style="background-color: white; border:0" onclick="DescargarDocumento(\'' + NombreArchivo + '\')">'
                html_row += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-download" viewBox="0 0 16 16" >'
                html_row += ' <path d="M.5 9.9a.5.5 0 0 1 .5.5v2.5a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1v-2.5a.5.5 0 0 1 1 0v2.5a2 2 0 0 1-2 2H2a2 2 0 0 1-2-2v-2.5a.5.5 0 0 1 .5-.5"/>'
                html_row += '<path d="M7.646 11.854a.5.5 0 0 0 .708 0l3-3a.5.5 0 0 0-.708-.708L8.5 10.293V1.5a.5.5 0 0 0-1 0v8.793L5.354 8.146a.5.5 0 1 0-.708.708z"/>'
                html_row += '</svg>'
                html_row += '</button>'
                
                html_row += '</td>';
             
                index++;
             }
             html_row += '</tbody>';
             html_row += '</table>';
             html_row += '</div>';
         }
         else {
            $('#data').empty();

            html_row = '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
            html_row += '  <div class="col-sm-12 xsmall text-center" style="border 1px solid;">'
            html_row += '     <span>No se encontraron registros para el ' + formattedDateScreen + ' actualizado a las ' + formattedTimeScreen + '<span>';
            html_row += '  </div>';
            html_row += '</div>';
         }

           $('#data').append(html_row);

      } else {
         $('#data').empty();

         html_row += '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
         html_row += '  <div class="col-sm-12 xsmall" style="border 1px solid;">'
         html_row += '     <span>Error al consultar a la API<span>';
         html_row += '  </div>';
         html_row += '</div>';

         $('#data').append(html_row);
      }

   }).fail(function (jqXHR, textStatus, errorThrown) {
            
      $('#message').html('<span class="text-danger blink">falla en la conexión ' + VerifyErrorType(jqXHR) +'</span>');
   });
   
}

function LoadLogsDetail(app_id, date, id_log_type, search) {

   $.ajax({
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      url: url_api_logs_detail,
      data: JSON.stringify({
         'id_aplicacion': app_id,
         'fecha': date,
         'id_tipo_log': id_log_type,
         'buscar': search
      }),
      dataType: "json"
   }).done(function (data) {

      if (data != "") {

         let exists = Object.keys(data).includes('response');

         if (exists) {
            $('#data-detail').empty();

            html_row = '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
            html_row += '  <div class="col-sm-12 xsmall" style="border 1px solid;">'
            html_row += '     <span>Se produjo el siguiente error<span></br>';
            html_row += '     <span>' + data.description + '<span>';
            html_row += '  </div>';
            html_row += '</div>';

            $('#data-detail').append(html_row);
            return;
         }

         $('#data-detail').empty();

         /*var records = JSON.parse(data);*/
         var index = 0;
         var records_count = 0;
         //var cantidad_columnas = CANT_COLUMNAS + 1;  // Para forzar la primer fila
         var html_row = '';
         var color = '';

         var id = '';
         var data_id_application = '';
         var date = '';
         var id_log_type = '';
         var description = '';
         var source = '';
         var package_description = '';
         var error_description = '';
         var response_description = '';
         var group_code = '';
         var log_type = '';
         var id_estado_log = '';
         var descripcion_estado = '';
         var clave_estado = '';

         //if (data.items.length > 0) {
         //   html_row = '<br/>';
         //   html_row += '<div class="row small" style="border:1px solid; border-color:#BBB2B0; padding-bottom:2px;padding-top:2px;" >';
         //   html_row += '     <div class="col-md-1"><span class="text-bold">ID</span></div>';            
         //   html_row += '     <div class="col-md-8"><span class="text-bold">Descripción</span></div>';
         //   html_row += '     <div class="col-md-3"><span class="text-bold">Origen</span></div>';
         //   html_row += '</div>';
         //}

         //var ciclos = 0;
         records_count = data.count;

         if (records_count > 0) {

            let record = '';
            

            while (index < data.items.length) {

               records_count++;

               id = data.items[index].id;
               data_id_application = data.items[index].id_aplicacion;
               date = data.items[index].fecha;
               id_log_type = data.items[index].id_tipo_log;
               description = data.items[index].descripcion;
               source = data.items[index].procedencia;
               package_description = data.items[index].descripcion_paquete;
               error_description = data.items[index].descripcion_error;
               response_description = data.items[index].descripcion_respuesta;
               group_code = data.items[index].codigo_agrupador;
               log_type = '';

               id_estado_log = data.items[index].id_estado_log;
               descripcion_estado = data.items[index].descripcion_estado_log;
               clave_estado = data.items[index].clave_estado_log;

               // Almacena los datos en un registro
               record = {
                  id: data.items[index].id,
                  id_application : data.items[index].id_aplicacion,
                  date : data.items[index].fecha,
                  id_log_type : data.items[index].id_tipo_log,
                  description : data.items[index].descripcion,
                  source : data.items[index].procedencia,
                  package_description : data.items[index].descripcion_paquete,
                  error_description : data.items[index].descripcion_error,
                  response_description : data.items[index].descripcion_respuesta,
                  group_code : data.items[index].codigo_agrupador,
                  log_type : '',
                  id_estado_log : data.items[index].id_estado_log,
                  descripcion_estado : data.items[index].descripcion_estado_log,
                  clave_estado : data.items[index].clave_estado_log
               };
               
               records.push(record);

               color = 'text-info';

               //if (level === 1) color = 'text-info';
               //if (level === 2) color = 'text-warning';
               //if (level === 3) color = 'text-danger';

               html_row += '<div id="' + id + '" class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';

               html_row += '<div class="col-md-1 xsmall" style="border 0px solid;">'
               html_row += '     <span class="xxsmall text-dark">ID:</span><br/><span>' + id + '<span>';
               html_row += '</div>';

               //html_row += '<div class="col-lg-2 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span>' + date + '<span>';
               //html_row += '</div>';

               html_row += '<div class="col-md-7 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">DESCRIPCION:</span><br/><span>' + error_description + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-md-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">ORIGEN:</span><br/><span>' + source + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-md-2 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">ESTADO:</span><br/><span>' + descripcion_estado + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-sm-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span id="lbl_' + id + '" class="xxsmall text-dark">' + Get_legend_status(clave_estado) + '</span><br/>';
               html_row += '     <input type="checkbox"/ id="log_' + id + '" onclick="ChangeStateLog(this,' + id + ',\'' +clave_estado +'\');" ' + (Get_check_status(clave_estado) ?'checked':'') + '>';
               html_row += '</div>';
               //html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span ' + (level == 3 ? 'class="blink"' : '') + ' >' + critical + '<span>';
               //html_row += '</div>';

               //html_row += '<div class="col-lg-2 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span>' + error_description + '<span>';
               //html_row += '</div>';

               //html_row += '<div class="col-lg-2 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span>' + response_description + '<span>';
               //html_row += '</div>';
               //html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span style="cursor:pointer;" onclick="alert(' + id + ');">' + log_type + '<span>';
               //html_row += '</div>';
              

               html_row += '   </div>';
               index++;
            }
            
            html_row += '<div class="row mt-4"><div class="col-sm-12"><button class="btn btn-light small w-100 border border-primary" style="height:25px;padding-top:0px;" onclick="DownloadLogDetail()"><span class="small">Descargar Logs</span></button></div></div>';

            // Cierra la fila
            //if (records.items.length > 0)
            html_row += '</div>';

         } else {
            $('#data-detail').empty();

            html_row = '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
            html_row += '  <div class="col-sm-12 xsmall text-center" style="border 1px solid;">'
            html_row += '     <span>No se encontraron registros para el ' + formattedDateScreen + ' actualizado a las ' + formattedTimeScreen +  '<span>';
            html_row += '  </div>';
            html_row += '</div>';

         }

         $('#data-detail').append(html_row);
         
      } else {
         $('#data-detail').empty();

         html_row += '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
         html_row += '  <div class="col-sm-12 xsmall" style="border 1px solid;">'
         html_row += '     <span>Error al consultar a la API<span>';
         html_row += '  </div>';
         html_row += '</div>';

         $('#data-detail').append(html_row);
      }

   }).fail(function (d) {

      $('#message').html('<span class="text-danger blink">Falla en la conexión a BD ' + VerifyErrorType(jqXHR) + ' </span><br/><span>d</span>');
   });

}
function DescargarDocumento(nombre_documento) {
    var _nombre_documento = nombre_documento.replace(":", '');

    $.ajax({
        async: false,
        url: url_api_savedocument,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify( nombre_documento ),
        success: (function (data) {
            var blob = new Blob([data], { type: 'application/octet-stream' });
            var url = window.URL.createObjectURL(blob);
            var link = document.createElement('a');
            link.href = url;
            link.download = _nombre_documento;
            document.body.appendChild(link);
            link.click();
            window.URL.revokeObjectURL(link);
        })
    })
}


function LoadResponsable() {
   var a = '';

    $.ajax({
      async: false,
      type: 'GET',
      contentType: 'application/json; charset=utf-8',
      url: url_api_responsable,
      //data: JSON.stringify({
      //   'id_tipo_log': id_tipo_log,
      //   'fecha': fecha
      //}),
      dataType: "json"
   }).done(function (data) {

       if (data != "") {
           responsables = data.items;

         // var index = 0;
         //var id = '';
         //var estado = '';

         //var html_row_respon = '';

         ////var ciclos = 0;
         //records_count = data.count;

         //while (index < data.items.length) {

         //   records_count++;
         //   if (index == 0)
         //       html_row_respon += '<option value="0">Todos</option>';
         //    id = data.items[index].id;
         //    estado = data.items[index].DescripEstado;

         //    html_row_respon += '<option value="' + id +'">'+estado+'</option>';
         //   index++;
         //}

      } 

   }).fail(function (d) {

      $('#message').html('<span class="text-danger blink">falla en la conexion con responsables</span>');
   });
}

function LoadEstados() {
    var a = '';

    $.ajax({
        async: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        url: url_api_apps,
        //data: JSON.stringify({
        //   'id_tipo_log': id_tipo_log,
        //   'fecha': fecha
        //}),
        dataType: "json"
    }).done(function (data) {

        if (data != "") {

            estados = data.items;
            $('#combo_app').empty();

            var index = 0;

            var id = '';
            var estado = '';

            var html_row_estado = '';

            //var ciclos = 0;
            records_count = data.count;

            while (index < data.items.length) {

                records_count++;
                if (index == 0)
                    html_row_estado += '<option value="0">Todos</option>';
                id = data.items[index].IdEstado;
                estado = data.items[index].DescripEstado;

                html_row_estado += '<option value="' + id + '">' + estado + '</option>';
                index++;
            }

            $('#combo_app').html(html_row_estado);

        } else {
            $('#combo_app').empty();

        }

    }).fail(function (d) {

        $('#message').html('<span class="text-danger blink">falla en la conexión aplicaciones</span>');
    });
}

function LoadIntegracion() {
   var a = '';

   $.ajax({
      type: 'GET',
      contentType: 'application/json; charset=utf-8',
      url: url_api_logtype,
      //data: JSON.stringify({
      //   'id_tipo_log': id_tipo_log,
      //   'fecha': fecha
      //}),
      dataType: "json"
   }).done(function (data) {

      if (data != "") {
          $('#combo_integracion').empty();

          var index = 0;
          var html_row = '';
          var id = '';
          var description = '';

         records_count = data.count;

         while (index < data.items.length) {

            records_count++;

            if (index == 0) {
               html_row += '<option value="0">Todos</option>';
            }

            id = data.items[index].IdIntegracion;
            description = data.items[index].DescripIntegra;

            //html_row += '<span class="dropdown-item cursor-pointer" id="' + id + '" onclick="SelectType(' + id + ',`' + description +'`)">' + description + '</span>';
            html_row += '<option value="' + id + '"  >' + description + '</option>';

            index++;
         }

         //$('#typelog').append(html_row);

          $('#combo_integracion').html(html_row);


      } else {
         $('#combo_typelog').empty();

      }

   }).fail(function (d) {

      $('#message').html('<span class="text-danger blink">falla en la conexión aplicaciones</span>');
   });
}

function onChange(docu) {
    var idDocu = $(docu).attr('iddocu')
    $("button[id=edit" + idDocu + "]").attr("disabled", false);
}

function EnableButton(idDocu) {
    $("button[id=edit" + idDocu + "]").attr("disabled", false);
}

//$('#guardar-observacion').click(function () {
//    $("td[id=obs" + idDocumento + "]").attr("detail").val($(".observacion").val());
//});

function ShowObservacion(idDocumento) {
    let observacion = $("td[id=obs" + idDocumento + "]").attr("detail");
    $(".observacion").val(observacion);
    $(".observacion").attr("idDocumentoEdit", idDocumento);
}

function GuardarCambio(idDocu) {
    //var idDocu = $(docu).attr('iddocu')
    var responsEdit = $("select.responsable[iddocu=" + idDocu + "]").val();
    var estadoEdit = $("select.estado[iddocu=" + idDocu + "]").val();
    var observacionEdit = $("td[id=obs" + idDocu + "]").attr("detail");
    
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: url_api_savechanges,
        data: JSON.stringify({
            'id_docu': idDocu,
            'id_estado': estadoEdit,
            'id_responsable': responsEdit,
            'observacion': observacionEdit
        }),
        dataType: "json"
    }).done(function (data) {
        if (data != "") {
            $("button[id=edit" + idDocu + "]").attr("disabled", true);
            //$("#contenedor").load("#contenedor");
            setInterval(LoadErrores("0"), 0);
        }
    }).fail(function (fail) {
        PopupDetail('popup_message', 'No fue posible actualizar el estado del log ' + id, 'data-detail', 'sm');

    });
}

function ChangeStateLog(control, id, clave_estado) {

   var nueva_clave = Get_alternate_status(clave_estado);

   $.ajax({
      type: 'PATCH',
      contentType: 'application/json; charset=utf-8',
      url: url_api_update_state,
      data: JSON.stringify({
         'id_log': id,
         'clave_estado': nueva_clave
      }),
      dataType: "json"
   }).done(function (data) {
      if (data != "") {
         let exists = Object.keys(data).includes('response');
         if (exists) {

            if ($(control).is(':checked')) {
               //alert(id + ' seleccionado');
               $('#lbl_' + id).text('Solucionado');
            }
            else {
               //alert(id + ' sin seleccion');
               $('#lbl_' + id).text('Pendiente');
            }
         }
      }
   }).fail(function (fail) {
      PopupDetail('popup_message', 'No fue posible actualizar el estado del log ' + id, 'data-detail', 'sm');
      
   });

}
function ShowClock() {

   var today = new Date();
  
   var s = today.getSeconds();
   var m = today.getMinutes();
   var h = today.getHours();

   
   const yyyy = today.getFullYear();
   let mm = today.getMonth() + 1; // Months start at 0!
   let dd = today.getDate();

   if (dd < 10) dd = '0' + dd;
   if (mm < 10) mm = '0' + mm;

   var formattedToday = dd + '/' + mm + '/' + yyyy;

   var textContent = formattedToday + ' ' +
      ("0" + h).substr(-2) + ":" + ("0" + m).substr(-2) + ":" + ("0" + s).substr(-2);

   $('#clock').html('<span>Hoy ' + textContent + '</span>');
}

function DescargarArchivo(idDocu) {
    var responsEdit = $("select.responsable[iddocu=" + idDocu + "]").val();
    var estadoEdit = $("select.estado[iddocu=" + idDocu + "]").val();
    var observacionEdit = $("td[id=obs" + idDocu + "]").attr("detail");

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        url: url_api_savechanges,
        data: JSON.stringify({
            'id_docu': idDocu,
            'id_estado': estadoEdit,
            'id_responsable': responsEdit,
            'observacion': observacionEdit
        }),
        dataType: "json"
    }).done(function (data) {
        if (data != "") {
            $("button[id=edit" + idDocu + "]").attr("disabled", true);
            //$("#contenedor").load("#contenedor");
            setInterval(LoadErrores("0"), 0);
        }
    }).fail(function (fail) {
        PopupDetail('popup_message', 'No fue posible actualizar el estado del log ' + id, 'data-detail', 'sm');

    });
}

function SetDateToSearch(sdate) {

   yyyy = today.getFullYear();
   mm = today.getMonth() + 1; // Months start at 0!
   dd = today.getDate();

   if(sdate !== undefined && sdate!=='')
   {
      let [day, mon, year] = sdate.split('/');
      dd = day;
      mm = mon;
      yyyy = year;
   }
   
   formattedDateSQL = yyyy + '-' + MM + '-' + dd;
   formattedDateScreen = pad(2, dd, '0') + '/' + pad(2, MM, '0') + '/' + yyyy;
   //formattedTimeScreen = pad(2, hh, '0') + ':' + pad(2, mm, '0') + ':' + pad(2, ss, '0');
                          
}

function SelectType(id, name) {
   type_option = id;
   if(name!==undefined)
      $("#dropdownMenuButton").html('Tipo de mensajes :' + name);
   //let id_app = $("#combo_app option:selected").val();
   LoadLogs();
}

function pad(width, string, padding) {
   inputData = String(string)
   return (width <= inputData.length) ? inputData : pad(width, padding + inputData, padding)
}

function ShowLogsDetails(app_id, name, date, id_log_type, search) {

   let datesql = '';
   

   // Validaciones de parametros
   if (date === '' || date === undefined)
      datesql = formattedDateSQL;
   else {
      if (date.includes('/')) {
         let [day, month, year] = date.split('/')
         datesql = year + '-' + month + '-' + day;
      } else
         datesql = date;

   }
   //alert(app_id + ' ' + log_type);
   //alert(app_id + '  search:' + search);
   //id_app, date,id_log_type,search
   LoadLogsDetail(app_id, datesql, id_log_type, search)

   // Llamada al popup de la Common.js
   PopupDetail('popup_log_detail', 'Detalle de logueos de ' + name, 'data-detail','xl');
}

function GetHtmlSpinner() {
   let html = '';

   html = '<div class="row">';
   html += '   <div class="col-lg-12 text-center">';
   html += '      <div class="spinner-border text-primary" role="status">';
   html += '         <span class="sr-only">Cargando datos...</span>';
   html += '      </div>';
   html += '   </div>';
   html += '</div>';

   return (html);
}

function Get_legend_interval(interval) {

   var html = ''

   var hours = Math.floor(interval / 3600);
   var minutes = Math.floor((interval % 3600) / 60);
   var remainingSeconds = interval % 60;
   return (hours > 0 ? hours + ' horas ' : '') + (minutes > 0 ? minutes + ' minutos ' : '') + (remainingSeconds > 0 ? remainingSeconds + ' segundos':'');

   
   return html;
}

function Get_legend_status(key) {
   var desc = '';
   switch (key) {
      case 'PEND':
         desc = Get_legend_html_colored('Pendiente','#ff0000');
         break;
      case 'REVI':
         desc = Get_legend_html_colored('Revisión','#ffccff');
         break;
      case 'SOLU':
         desc = Get_legend_html_colored('Solucionado','#00ff00');
         break;
   }
   return desc;
}

function Get_legend_html_colored(text,color) {
   return '<span style="color:'+color+'">'+text+'</span>'
}

function Get_alternate_status(key) {
   var newkey = '';
   switch (key) {
      case 'PEND':
      case 'REVI':
         newkey = 'SOLU';
         break;
      case 'SOLU':
         newkey = 'PEND';
         break;
   }
   return newkey;
}
function Get_check_status(key) {

   switch (key) {
      case 'PEND':
         return false;
         break;
      case 'REVI':         
      case 'SOLU':
         return true;
         break;
   }
}

function DownloadLogDetail() {
   
   //console.log(records);
   //alert('descargando...');
   let data = '';
   records.forEach((item) => {

      data += item.id + ' ' + item.error_description + ' '+ item.descripcion_estado + '\n';
   })
   var textFile = new Blob([data], {
      type: 'text/plain'
   });
   invokeSaveAsDialog(textFile, 'TextFile.txt');
}

function VerifyErrorType(jqXHR) {

   if (jqXHR.status === 0) {

      message = 'Verificar conectividad de red';

   } else if (jqXHR.status == 404) {

      message = 'Recurso no encontrado [404]';

   } else if (jqXHR.status == 500) {

      message = 'Internal Server Error [500].';

   } else if (textStatus === 'parsererror') {

      message = 'Requested JSON parse failed.';

   } else if (textStatus === 'timeout') {

      message = 'Error de Time Out';

   } else if (textStatus === 'abort') {

      message = 'Requerimiento Ajax abortedo';

   } else {

      message = 'Uncaught Error: ' + jqXHR.responseText;

   }
   return (message);
}

function GuardarObservacion() {
    let observacion = $("input[iddocumentoedit]").val();
    let idDocu = $("input[id=input-observacion]").attr("iddocumentoedit");
    $("td[id=obs" + idDocu + "]").attr("detail", observacion);
    $("#ex1 .close-modal").click()
    GuardarCambio(idDocu);
    //setInterval(LoadErrores("0"), 0);
}
