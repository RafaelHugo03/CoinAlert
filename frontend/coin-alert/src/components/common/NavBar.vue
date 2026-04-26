<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <nav class="navbar">
    <div class="navbar-brand">
      <span class="brand-icon">₿</span>
      <span class="brand-name">CoinAlert</span>
    </div>
    <div class="navbar-links">
      <RouterLink to="/" class="nav-link">Dashboard</RouterLink>
      <RouterLink to="/opportunities" class="nav-link">Opportunities</RouterLink>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 2rem;
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
}

.navbar-brand {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.brand-icon {
  font-size: 1.5rem;
  color: var(--color-primary);
}

.brand-name {
  font-size: 1.25rem;
  font-weight: 700;
  color: var(--color-text);
  letter-spacing: 0.5px;
}

.navbar-links {
  display: flex;
  gap: 1.5rem;
}

.nav-link {
  color: var(--color-text-soft);
  text-decoration: none;
  font-size: 0.9rem;
  font-weight: 500;
  padding: 0.4rem 0.75rem;
  border-radius: 6px;
  transition: all 0.15s;
}

.nav-link:hover {
  color: var(--color-text);
  background: var(--color-border);
}

.nav-link.router-link-exact-active {
  color: var(--color-primary);
  background: rgba(59, 130, 246, 0.1);
}
</style>
