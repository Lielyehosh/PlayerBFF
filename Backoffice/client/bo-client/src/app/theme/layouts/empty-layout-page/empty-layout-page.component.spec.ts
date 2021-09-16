import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmptyLayoutPageComponent } from './empty-layout-page.component';

describe('EmptyLayoutPageComponent', () => {
  let component: EmptyLayoutPageComponent;
  let fixture: ComponentFixture<EmptyLayoutPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmptyLayoutPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmptyLayoutPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
