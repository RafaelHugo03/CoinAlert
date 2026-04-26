<script setup lang="ts">
import { onMounted } from 'vue'
import { RouterView } from 'vue-router'
import NavBar from './components/common/NavBar.vue'
import { useSignalR } from './composables/useSignalR'
import { useCryptoStore } from './stores/cryptoStore'

const { init: initSignalR } = useSignalR()
const cryptoStore = useCryptoStore()

onMounted(async () => {
  await Promise.all([cryptoStore.init(), initSignalR()])
})
</script>

<template>
  <NavBar />
  <RouterView />
</template>
