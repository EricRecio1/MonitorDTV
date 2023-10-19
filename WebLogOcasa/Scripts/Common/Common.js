const { Modal } = require("bootstrap");


function PopupDetail(popup_control_id,title,data_detail_control_id,modal_size) {

   var html = '';
   var size = ''
   if (modal_size == undefined) modal_size = '';
   size = modal_size;

   html = '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
   switch (modal_size) {
      case "sm":
         size = 'modal-sm';
         break;
      case "md":
         size = '';
         break;
      case "lg":
         size = 'modal-lg';
         break;
      case "xl":
         size = 'modal-xl';
         break;
   }
   
   html += '      <div class="modal-dialog ' + size + '">';
   html += '         <div class="modal-content">';
   html += '            <div class="modal-header">';
   html += '               <h6 class="modal-title text-title" id="myModalLabel">' + (title === '' || title===undefined ?'Detalle de logs':title)+'</h6>';
   html += '               <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';

   html += '            </div>'; 
   html += '            <div class="modal-body overflow-auto" style="max-height:400px;">';
   html += '               <div class="xsmall" id="' + data_detail_control_id + '"></div>';
   html += '            </div>';
   html += '            <div class="modal-footer">';
   html += '               <button type="button" class="btn btn-default text-normal" data-dismiss="modal">Cerrar</button>';
   html += '            </div>';
   html += '         </div>';
   html += '      </div>';
   html += '</div>';

   

   $('#'+popup_control_id).html(html);
   $('#myModal').modal("show");
   
}