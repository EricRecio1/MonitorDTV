function CreateSQLScriptToCheckLog() {

   var textFile = new Blob(['Hola mundo'], {
      type: 'text/plain'
   });
   invokeSaveAsDialog(textFile, 'TextFile.txt');

}