<template>
  <v-layout align-center class="ma-12 pa-12" column>
    <h1 class="font-weight-normal">
      <i class="red--text"> {{ $route.params.pathMatch }}</i> could not be
      found!
    </h1>
    <v-img
      :src="require(`@/assets/images/not_found.svg`)"
      class="ma-3"
      width="30%"
    ></v-img>
  </v-layout>
</template>
