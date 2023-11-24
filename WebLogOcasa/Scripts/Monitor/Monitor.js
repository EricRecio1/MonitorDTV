// V a r i a b l e s    p u b l i c a s


// PRD
var url_api_logs = 'https://monitordelogsapi.ocasa.com/api/ListLogs';
var url_api_logtype = 'https://monitordelogsapi.ocasa.com/api/ListTypesLog';
var url_api_logs_detail = 'https://monitordelogsapi.ocasa.com/api/ListLogDetail';
var url_api_apps = 'https://monitordelogsapi.ocasa.com/api/ListApplication';
var url_api_update_state = 'https://monitordelogsapi.ocasa.com/api/UpdateLogState';

// DEVELOP
/*
var url_api_logs = 'https://localhost:44361/api/ListLogs';
var url_api_logtype = 'https://localhost:44361/api/ListTypesLog';
var url_api_logs_detail = 'https://localhost:44361/api/ListLogDetail';
var url_api_apps = 'https://localhost:44361/api/ListApplication';
var url_api_apps = 'https://localhost:44361/api/UpdateLogState';
*/

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

SetDateToSearch(dd+'/'+mm+'/'+yyyy);



$(document).ready(function () {
   InitializeControls();
   
});

// F U N C I O N E S 

$(function () {
   /*$('#datepicker').defaults = {
      dayViewHeaderFormat: 'MMMM YYYY',
      locale: 'es',
      endDate: '+1d',
      todayHighlight: true,
      format: true,
      autoclose: true
   }*/
   $('#datepicker').datepicker({
      locale: 'es',
      autoclose: true,
      todayHighlight: true,
      autoclose: true,
      endDate: '+1d',
      datesDisabled: '+1d',
      keyboardNavigation: true
   });
});


function InitializeControls() {

   LoadLogType();
   LoadApps();
   LoadLogs();
   // Setea el intervalo para la consulta del log
   idLogInterval=setInterval(LoadLogs, refresh_interval * 1000);
   // Setea el intervalo para la consulta del reloj
   idClockInterval=setInterval(ShowClock, refresh_clock_interval * 1000);
   

   interval_legend = Get_legend_interval(refresh_interval);
   $('#intervalo').html(interval_legend);

   $('#btnReload').click(function () {      
      LoadLogs();
   });
   $('#datepicker').change(function (e) {
      SetDateToSearch($('#fecha').val());      
      LoadLogs();
      
   });
  
   

   $('#tInterval').change(function () {
      refresh_interval = this.value;
      interval_legend = Get_legend_interval(refresh_interval);
      $('#intervalo').html(interval_legend);

      clearInterval(idLogInterval);
      SetLogParameter();
      idLogInterval=setInterval(LoadLogs, refresh_interval * 1000);
   });
   $('#tInterval').on('input', function () {
      refresh_interval = this.value;
      interval_legend = Get_legend_interval(refresh_interval);
      $('#intervalo').html(interval_legend);
   });

   $('.combobox').combobox();

   // bonus: add a placeholder
   $('.combobox').attr('placeholder', 'Nombre aplicación');
   $('#combo_app').change(function () {
      LoadLogs();
   });
   $('#combo_typelog').change(function () {
      LoadLogs();
   });
}

function LoadLogs() {

   var html = '';
   // Obtiene los valores de los parámetros
   var id_logtype = (type_option !== id_logtype && type_option !== undefined ? type_option:id_logtype);
   var date = $('#fecha').val();

   id_application = $("#combo_app option:selected").val();
   id_logtype = $("#combo_typelog option:selected").val();
  
   let datesql = '';
   // Validaciones de parametros
   if (date === '' || date === undefined)
      datesql = formattedDateSQL;
   else {
      let [day, month, year] = date.split('/')
      datesql = year + '-' + month + '-' + day;
   }

   if (id_logtype === '' || id_logtype === undefined)
      id_logtype = 0;

   html = GetHtmlSpinner();
   $('#data').html(html);

   $('#message').html('<span class="text-primary">Consultando logs del ' + formattedDateScreen +'</span>');

   $.ajax({
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      url: url_api_logs,
      data: JSON.stringify({
         'id_tipo_log': id_logtype,
         'fecha': datesql,
         'id_aplicacion': id_application
      }),
      dataType: "json"
   }).done(function (data) {

      $('#data').append('');

      if (data != "") {

         /*var records = JSON.parse(data);*/
         var index = 0;
         var records_count = 0;
         //var cantidad_columnas = CANT_COLUMNAS + 1;  // Para forzar la primer fila
         var html_row = '';
         var color = '';

         var id = '';
         var name = '';
         var active = '';
         var description = '';
         var max_error_message = 0;
         var id_log_type = 0;
         var log_count = 0;
         var critical = '';
         var level = 0;
         var server = '';
         var log_type = '';
         var search = '';

         var actual_time = new Date();
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
         let exists = Object.keys(data).includes('response');
         if (exists) {
            $('#data').empty();

            html_row = '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
            html_row += '  <div class="col-sm-12 xsmall" style="border 1px solid;">'
            html_row += '     <span>Se produjo el siguiente error<span></br>';
            html_row += '     <span>' + data.description + '<span>';
            html_row += '  </div>';
            html_row += '</div>';

            $('#data').append(html_row);
            return;
         }

         if (data.items.length > 0) {

            html_row = '<br/>';
            html_row += '<div class="row xxsmall" style="border:1px solid; border-color:#BBB2B0; padding-bottom:2px;padding-top:2px;" >';
            html_row += '     <div class="col-lg-12"><span class="text-bold">Actualizado a las ' + formattedTimeScreen +'</span></div>';
            html_row += '</div>';
         //   html_row += '<div class="row small" style="border:1px solid; border-color:#BBB2B0; padding-bottom:2px;padding-top:2px;" >';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">ID</span></div>';
         //   html_row += '     <div class="col-lg-2"><span class="text-bold">NOMBRE</span></div>';
         //   //html_row += '     <div class="col-sm-1"><span class="text-bold">Activo</span></div>';
         //   html_row += '     <div class="col-lg-4"><span class="text-bold">Descripcion</span></div>';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">Max. mjes</span></div>';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">Criticidad</span></div>';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">Servidor</span></div>';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">Mjes.</span></div>';
         //   html_row += '     <div class="col-lg-1"><span class="text-bold">Tipo</span></div>';
         //   html_row += '</div>';

         }


         //var ciclos = 0;
         records_count = data.count;

         if (records_count > 0) {

            while (index < data.items.length) {

               records_count++;

               id = data.items[index].id;
               name = data.items[index].nombre;
               active = data.items[index].activo;
               description = data.items[index].descripcion;
               max_error_message = data.items[index].max_mensajes_error;
               id_log_type = data.items[index].id_tipo_log;
               log_count = data.items[index].cantidad_log;
               critical = data.items[index].criticidad;
               level = data.items[index].nivel;
               server = data.items[index].servidor;
               log_type = data.items[index].tipo_log;

               color = 'text-info';

               if (level === 1) color = 'text-info';
               if (level === 2) color = 'text-warning';
               if (level === 3) color = 'text-danger';

               html_row += '<div id="' + id + '" class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';

               html_row += '<div class="col-lg-1 xsmall" style="border 0px solid;">'
               html_row += '     <span class="xxsmall text-dark">ID:</span><br/><span>' + id + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-lg-2 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">NOMBRE:</span><br/><span style="cursor:pointer;">' + name + '<span>';
               html_row += '</div>';

               //html_row += '<div class="col-sm-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span>' + (active == true ? 'Activo' : '-') + '<span>';
               //html_row += '</div>';

               html_row += '<div class="col-lg-4 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">DESCRIPCION:</span><br/><span>' + description + '<span>';
               html_row += '</div>';

               //html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               //html_row += '     <span class="xxsmall text-dark">MAX MJS:</span><br/><span class="text-center">' + max_error_message + '<span>';
               //html_row += '</div>';

               html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">CRITICIDAD:</span><br/><span ' + (level == 3 ? 'class="blink"' : '') + ' >' + critical + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">SERVIDOR:</span><br/><span>' + server + '<span>';
               html_row += '</div>';

               html_row += '<div class="col-lg-1 xsmall text-right" style="border 1px solid; border-color:#BBB2B0;">'
               html_row += '     <span class="xxsmall text-dark">MJES:</span><br/><span class="h6">' + log_count + '<span>';
               html_row += '</div>';
               html_row += '<div class="col-lg-1 xsmall" style="border 1px solid; border-color:#BBB2B0; cursor:pointer;" onclick="ShowLogsDetails(' + id + ',\'' + name + '\',\'' + datesql + '\',' + id_log_type + ',\'' + search + '\');">'
               html_row += '     <span class="xxsmall text-dark">TIPO:</span><br/><span>' + log_type + '<span>';
               html_row += '</div>';                                                                           // app_id, date, log_type, search
               //         html_row += '<div class="col-sm-3" style="border:0px solid; border-color:#000000; padding-left:5px; padding-right:5px; width:350px;">';


               //         html_row += '       <div class="row" style="border:0px solid; border-color:#d1cbcc; height:30%; max-height:30%;">';
               //         html_row += '           <div class="col-sm-12"></div>';
               //         html_row += '       </div>';
               //         html_row += '       <div class="row" style="border:0px solid; border-color:#d1cbcc; height:30%; max-height:30%;">';
               //         html_row += '           <div class="col-sm-12">';
               //         html_row += '               <span class="shadow" style="font-size:18pt; font-weight:bold; background-color:rgba(255, 0, 0, 0.3); color:#ffffff; text-shadow: 2px 2px 3px #000000;">&nbsp;' + name + '&nbsp;</span>';
               //         html_row += '           </div>';
               //         html_row += '       </div>';
               //         html_row += '       <div class="row" style="border:0px solid; border-color:#d1cbcc; height:30%; max-height:30%;display: block;">';
               //         html_row += '           <div class="col-sm-12" style="text-align:right;"><span class="Publi_text bold" style="background-color:rgba(255, 0, 0, 0.3); color:#ffffff; text-shadow: 2px 2px 3px #000000;">&nbsp;&nbsp;' + Cantidad + '&nbsp;&nbsp;</span></div>';
               //         html_row += '       </div>';
               //         html_row += '       <div class="overlay" >';
               //         html_row += '           <center>';
               //         html_row += '               <span style="color:#ffffff;"><strong>Seleccion&aacute; esta opci&oacute;n para ver los cursos de ' + name + '</strong></span>';
               //         html_row += '               <br/><br/><br/>';
               //         html_row += '               <span class="h3" style="color:#ffffff;"><strong>' + Cantidad + (Cantidad == 1 ? ' curso' : ' cursos') + ' disponibles</strong></span>';
               //         html_row += '           </center >';
               //         html_row += '       </div >;'

               html_row += '   </div>';
               index++;

            }

            // Cierra la fila
            //if (records.items.length > 0)
            html_row += '</div>';

         } else {
            $('#data').empty();

            html_row = '<div class="row ' + color + '" style="border: 1px solid; border-color:#BBB2B0;">';
            html_row += '  <div class="col-sm-12 xsmall text-center" style="border 1px solid;">'
            html_row += '     <span>No se encontraron registros para el ' + formattedDateScreen + ' actualizado a las ' + formattedTimeScreen + '<span>';
            html_row += '  </div>';
            html_row += '</div>';
          
         }

         $('#data').append(html_row);


         //var script_java =
         //   '<script>' +
         //   // Funcion para cursor sobre imagen
         //   '   function CargarPaginaCursos(id,name){' +
         //   '       localStorage.setItem(\'idSeleccionada\', id);' +
         //   '       localStorage.setItem(\'nameAreaSeleccionada\', name);' +
         //   '       window.location.href=\'./Cursos.aspx\'' +
         //   '       ' +
         //   '   }' +
         //   '</script>';

         //// Agrega el javascript para las acciones
         //$('#grillaAreas').append(script_java);

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

function LoadApps() {
   var a = '';

   $.ajax({
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

         $('#combo_app').empty();

         /*var records = JSON.parse(data);*/
         var index = 0;
         var records_count = 0;
         //var cantidad_columnas = CANT_COLUMNAS + 1;  // Para forzar la primer fila
         var html_row = '';
         /* var color = '';*/

         var id = '';
         var nombre = '';

         //var ciclos = 0;
         records_count = data.count;

         while (index < data.items.length) {

            records_count++;
            if (index == 0)
               html_row += '<option value="0">Todas las aplicaciones</option>';
            id = data.items[index].id;
            nombre = data.items[index].nombre;

            html_row += '<option value="' + id +'">'+nombre+'</option>';

            index++;

         }

         $('#combo_app').html(html_row);




      } else {
         $('#combo_app').empty();

      }

   }).fail(function (d) {

      $('#message').html('<span class="text-danger blink">falla en la conexión aplicaciones</span>');
   });
}
function LoadLogType() {
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

         $('#combo_typelog').empty();

         /*var records = JSON.parse(data);*/
         var index = 0;
         var records_count = 0;
         //var cantidad_columnas = CANT_COLUMNAS + 1;  // Para forzar la primer fila
         var html_row = '';
         /* var color = '';*/

         var id = '';
         var description = '';

         //var ciclos = 0;
         records_count = data.count;

         while (index < data.items.length) {

            records_count++;

            if (index == 0) {
               html_row += '<option value="0">Todos los tipos</option>';
            }


            id = data.items[index].id;
            description = data.items[index].description;

            //html_row += '<span class="dropdown-item cursor-pointer" id="' + id + '" onclick="SelectType(' + id + ',`' + description +'`)">' + description + '</span>';
            html_row += '<option value="' + id + '"  >' + description + '</option>';

            index++;

         }

         $('#typelog').append(html_row);

         $('#combo_typelog').html(html_row);


      } else {
         $('#combo_typelog').empty();

      }

   }).fail(function (d) {

      $('#message').html('<span class="text-danger blink">falla en la conexión aplicaciones</span>');
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
