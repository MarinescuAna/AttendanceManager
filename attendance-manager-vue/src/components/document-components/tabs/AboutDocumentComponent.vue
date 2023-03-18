<template>
    <v-container>
        <v-card-subtitle>
            Created at {{ document.creationDate }} (Last update at
            {{ document.updateDate }})
        </v-card-subtitle>
        <v-card-text>
            <p><strong>Course name:</strong> {{ document.courseName }}</p>
            <p>
                <strong>Specialization name:</strong> {{ document.specializationName }}
            </p>
            <p>
                <strong>Students' enrollment year:</strong>
                {{ document.enrollmentYear }}
            </p>
            <p>
                <strong>Maximum number of lessons that will be held:</strong>
                {{ document.maxNoLessons }}({{ document.noLessons }} held by now)
            </p>
            <p>
                <strong>Maximum number of laboratories that will be held:</strong>
                {{ document.maxNoLaboratories }}({{ document.noLaboratories }} held by now)
            </p>
            <p>
                <strong>Maximum number of seminaries that will be held:</strong>
                {{ document.maxNoSeminaries }}({{ document.noSeminaries }} held by now)
            </p>
        </v-card-text>
    </v-container>
</template>

<script lang="ts">
    import { DocumentFullViewModule } from "@/modules/document";
    import storeHelper from "@/store/store-helper";
    import Vue from "vue";
    export default Vue.extend({
        computed: {
            document: function (): DocumentFullViewModule {
                return storeHelper.documentStore.documentDetails;
            },
        },
    });
</script>