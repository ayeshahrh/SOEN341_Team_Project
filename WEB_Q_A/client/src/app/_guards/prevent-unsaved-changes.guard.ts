import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {

  constructor(private confirmService: ConfirmService) {}

  canDeactivate(component : any): Observable<boolean> | boolean {
      if (component.editForm?.dirty){
        return this.confirmService.confirm('Confirmation',
         'Are you sure you want to navigate away from this page? Unsaved changes will be lost.');
      }
      return true;
    }
  
}
