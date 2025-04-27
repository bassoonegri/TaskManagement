-- Habilitar extensão de UUID
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- Inserir Projetos
INSERT INTO "Projects" ("Id", "Name", "UserId", "CreatedAt")
VALUES 
(gen_random_uuid(), 'Projeto Desenvolvimento', gen_random_uuid(), NOW()),
(gen_random_uuid(), 'Projeto Marketing', gen_random_uuid(), NOW()),
(gen_random_uuid(), 'Projeto RH', gen_random_uuid(), NOW());

-- Inserir Tarefas para cada Projeto
DO $$
DECLARE 
    projectId UUID;
    taskId UUID;
BEGIN
    FOR projectId IN SELECT "Id" FROM "Projects" LOOP
        -- Tarefa 1
        taskId := gen_random_uuid();
        INSERT INTO "Tasks" ("Id", "Title", "Description", "Priority", "Status", "DueDate", "ProjectId", "CreatedAt")
        VALUES (taskId, 'Tarefa A - ' || projectId, 'Descrição A', 1, 0, NOW() + INTERVAL '5 days', projectId, NOW());

        -- Comentários da Tarefa 1
        INSERT INTO "Comments" ("Id", "Content", "CreatedByUserId", "CreatedAt", "TaskId")
        VALUES 
        (gen_random_uuid(), 'Comentário 1A', gen_random_uuid(), NOW(), taskId),
        (gen_random_uuid(), 'Comentário 2A', gen_random_uuid(), NOW(), taskId);

        -- Histórico da Tarefa 1
        INSERT INTO "TaskHistories" ("Id", "TaskId", "ChangedAt", "ChangedByUserId", "ChangeDescription")
        VALUES 
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 1A'),
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 2A');

        -- Tarefa 2
        taskId := gen_random_uuid();
        INSERT INTO "Tasks" ("Id", "Title", "Description", "Priority", "Status", "DueDate", "ProjectId", "CreatedAt")
        VALUES (taskId, 'Tarefa B - ' || projectId, 'Descrição B', 2, 1, NOW() + INTERVAL '10 days', projectId, NOW());

        -- Comentários da Tarefa 2
        INSERT INTO "Comments" ("Id", "Content", "CreatedByUserId", "CreatedAt", "TaskId")
        VALUES 
        (gen_random_uuid(), 'Comentário 1B', gen_random_uuid(), NOW(), taskId),
        (gen_random_uuid(), 'Comentário 2B', gen_random_uuid(), NOW(), taskId);

        -- Histórico da Tarefa 2
        INSERT INTO "TaskHistories" ("Id", "TaskId", "ChangedAt", "ChangedByUserId", "ChangeDescription")
        VALUES 
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 1B'),
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 2B');

        -- Tarefa 3
        taskId := gen_random_uuid();
        INSERT INTO "Tasks" ("Id", "Title", "Description", "Priority", "Status", "DueDate", "ProjectId", "CreatedAt")
        VALUES (taskId, 'Tarefa C - ' || projectId, 'Descrição C', 3, 2, NOW() + INTERVAL '15 days', projectId, NOW());

        -- Comentários da Tarefa 3
        INSERT INTO "Comments" ("Id", "Content", "CreatedByUserId", "CreatedAt", "TaskId")
        VALUES 
        (gen_random_uuid(), 'Comentário 1C', gen_random_uuid(), NOW(), taskId),
        (gen_random_uuid(), 'Comentário 2C', gen_random_uuid(), NOW(), taskId);

        -- Histórico da Tarefa 3
        INSERT INTO "TaskHistories" ("Id", "TaskId", "ChangedAt", "ChangedByUserId", "ChangeDescription")
        VALUES 
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 1C'),
        (gen_random_uuid(), taskId, NOW(), gen_random_uuid(), 'Histórico 2C');
    END LOOP;
END $$;
