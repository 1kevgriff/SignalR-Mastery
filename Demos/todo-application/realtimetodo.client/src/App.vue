<template>
  <div id="app">
    <router-view />
  </div>
</template>

<style lang="scss">
</style>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class App extends Vue {
  async startConnection () {
    await this.$connectionService.start();
  }
  
  async created() {
    await this.startConnection();
  }
}
</script>
