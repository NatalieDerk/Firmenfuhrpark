import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartSeite } from './start-seite';

describe('StatrSeite', () => {
  let component: StartSeite;
  let fixture: ComponentFixture<StartSeite>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [StartSeite]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StartSeite);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
