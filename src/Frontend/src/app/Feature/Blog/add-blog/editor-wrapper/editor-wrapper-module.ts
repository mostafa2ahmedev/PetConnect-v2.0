import { NgModule } from '@angular/core';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [],
  imports: [QuillModule.forRoot()],
  exports: [QuillModule],
})
export class EditorWrapperModule {}
