import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminForm } from './admin-form';

describe('AdminForm', () => {
  var component: AdminForm;
  var fixture: ComponentFixture<AdminForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
