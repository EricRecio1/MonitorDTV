//const { Modal } = require("bootstrap");


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

/**
 * @param {Blob} file - File or Blob object. This parameter is required.
 * @param {string} fileName - Optional file name e.g. "image.png"
 */
function invokeSaveAsDialog(file, fileName) {
   if (!file) {
      throw 'Blob object is required.';
   }

   if (!file.type) {
      try {
         file.type = 'video/webm';
      } catch (e) { }
   }

   var fileExtension = (file.type || 'video/webm').split('/')[1];

   if (fileName && fileName.indexOf('.') !== -1) {
      var splitted = fileName.split('.');
      fileName = splitted[0];
      fileExtension = splitted[1];
   }

   var fileFullName = (fileName || (Math.round(Math.random() * 9999999999) + 888888888)) + '.' + fileExtension;

   if (typeof navigator.msSaveOrOpenBlob !== 'undefined') {
      return navigator.msSaveOrOpenBlob(file, fileFullName);
   } else if (typeof navigator.msSaveBlob !== 'undefined') {
      return navigator.msSaveBlob(file, fileFullName);
   }

   var hyperlink = document.createElement('a');
   hyperlink.href = URL.createObjectURL(file);
   hyperlink.download = fileFullName;

   hyperlink.style = 'display:none;opacity:0;color:transparent;';
   (document.body || document.documentElement).appendChild(hyperlink);

   if (typeof hyperlink.click === 'function') {
      hyperlink.click();
   } else {
      hyperlink.target = '_blank';
      hyperlink.dispatchEvent(new MouseEvent('click', {
         view: window,
         bubbles: true,
         cancelable: true
      }));
   }

   (window.URL || window.webkitURL).revokeObjectURL(hyperlink.href);
}