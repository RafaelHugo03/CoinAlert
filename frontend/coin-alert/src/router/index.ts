import { createRouter, createWebHistory } from 'vue-router'
import DashboardView from '../views/DashboardView.vue'
import OpportuinitiesView from '../views/OpportunitiesView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: DashboardView,
    },
    {
      path: '/opportunities',
      name: 'opportunities',
      component: OpportuinitiesView
    },
    {
      path: '/:pathMatch(.*)*',
      name: 'not-found',
      component: DashboardView
    }
  ],
})

export default router
