<template>
  <v-layout class="ma-3" column>
      <h1 class="font-weight-normal">
        <i> {{ $route.params.pathMatch }}</i> could not be found!
      </h1>
      <v-img
        :src="require(`@/assets/images/errors/not_found.png`)"
        class="ma-3"
        width="30%"
      ></v-img>
  </v-layout>
</template>
