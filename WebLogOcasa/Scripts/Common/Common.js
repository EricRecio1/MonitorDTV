

function PopupDetail(popup_control_id,title,data_detail_control_id) {

   var html = '';

    
   html = '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
   html += '      <div class="modal-dialog modal-lg">';
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