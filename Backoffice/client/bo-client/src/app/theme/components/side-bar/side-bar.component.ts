import { Component, OnInit } from '@angular/core';
import { NbMenuItem } from '@nebular/theme';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.scss']
})
export class SideBarComponent implements OnInit {
  items: NbMenuItem[] = [
    // {
    //   title: 'Profile',
    //   expanded: true,
    //   children: [
    //     {
    //       title: 'Change Password',
    //     },
    //     {
    //       title: 'Privacy Policy',
    //     },
    //     {
    //       title: 'Logout',
    //     },
    //   ],
    // },
    {
      title: 'Home',
      link: '/home'
    },
    {
      title: 'Settings',
      link: '/home/settings'
    }
  ];
  constructor() { }

  ngOnInit(): void {
  }

}
