import AuthService from '@/services/auth.service'
import { TokenData } from "@/shared/modules";
import { Role } from '@/shared/enums'
import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: '/',
    redirect: "/home"
  },
  {
    path: '/home',
    name: 'home',
    component: () => import('../views/HomeView.vue'),
    meta: {
      title: 'Home',
      requireAuth: false,
      role: [Role.All]
    },
  },
  {
    path: '/courses',
    name: 'courses',
    component: () => import('../views/courses-views/CourseView.vue'),
    meta: {
      title: 'Courses',
      requireAuth: true,
      role: [Role.Teacher]
    }
  },
  {
    path: '/users',
    name: 'users',
    component: () => import('../views/manage-users-views/UsersView.vue'),
    meta: {
      title: 'Users',
      requireAuth: true,
      role: [Role.Admin]
    },
  },
  {
    path: '/department',
    name: 'department',
    component: () => import('../views/departments-specializations-views/DepartmentView.vue'),
    meta: {
      title: 'Departments and Specializations',
      requireAuth: true,
      role: [Role.Admin]
    },
  },
  {
    path: '/create-document',
    name: 'create-document',
    component: () => import('../views/documents-views/CreateDocumentView.vue'),
    meta: {
      title: 'Create document',
      requireAuth: true,
      role: [Role.Teacher]
    },
  },
  {
    path: '/documents',
    name: 'documents',
    component: () => import('../views/documents-views/DocumentCardView.vue'),
    meta: {
      title: 'Documents',
      requireAuth: true,
      role: [Role.Teacher, Role.Student]
    },
  },
  {
    path: '/document/:id',
    name: 'document',
    component: () => import('../views/documents-views/DocumentDetailsView.vue'),
    meta: {
      title: 'Document',
      requireAuth: true,
      role: [Role.Teacher, Role.Student]
    },
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    component: () => import('../views/DashboardView.vue'),
    meta: {
      title: 'Dashboard',
      requireAuth: true,
      role: [Role.Teacher]
    },
  },
  {
    path: '/create-user',
    name: 'create-user',
    component: () => import('../views/manage-users-views/CreateUserView.vue'),
    meta: {
      title: 'Create User',
      requireAuth: true,
      role: [Role.Admin]
    },
  },
  {
    path: "/login",
    name: "login",
    component: () => import('../views/LoginView.vue'),
    meta: {
      title: 'Login',
      requireAuth: false,
      role: [Role.All]
    },
  },
  {
    // NotFound route
    // WARNING: DO NOT add any new route after this route, because it will never be matched!!!
    path: '/:pathMatch(.*)',
    component: () => import('../views/routing-views/NotFoundView.vue'),
    meta: {
      title: 'Page Not Found',
      requireAuth: false,
      role: [Role.All]
    },
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
  //set those two below properties in order to highlight the selected route
  linkActiveClass: "active",
  linkExactActiveClass: "active"
})

router.beforeEach(async (to, from, next) => {
  // Redirect user to login page each time when he try to access a page that needs authentication
  if (to.meta?.requireAuth) {
    const tokenData = AuthService.getDataFromToken();

    if (tokenData === null) {
      next({ name: 'login' });
    } else {
      if (to.meta?.role.filter(role=> Role[role] == (tokenData as TokenData).role.toString()).length>0) {
        next();
      }
    }
  } else {
    next();
  }
});

router.afterEach((to) => {
  if (to.meta?.title) {
    document.title = to.meta?.title;
  } else {
    document.title = "AttendanceManager";
  }
});

export default router
