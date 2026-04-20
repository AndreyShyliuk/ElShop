import { NgModule, Optional, SkipSelf } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

@NgModule({
  providers: [provideHttpClient(withInterceptorsFromDi())],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parent: CoreModule) {
    if (parent) throw new Error('CoreModule must only be imported once in AppModule.');
  }
}
