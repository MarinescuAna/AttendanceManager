<template>
  <button @click="test">Test</button>
</template>

<script lang="ts">
  import AuthService from '@/services/auth.service'
import Vue from 'vue'

  export default Vue.extend({

    methods:{
      test():void{
        AuthService.test();
      }
    }
  })
</script>
