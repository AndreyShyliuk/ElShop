# Ticket Workflow Skill

Follow this skill for every piece of work that corresponds to a GitHub issue.

## 1. Pick a ticket

List open, unassigned issues and pick the lowest-numbered one that has no `in progress` label (or take the one the user specifies):

```bash
gh issue list --repo AndreyShyliuk/ElShop --state open --assignee "" \
  --json number,title,labels \
  --jq '.[] | select(.labels | map(.name) | contains(["in progress"]) | not) | "#\(.number) \(.title)"'
```

## 2. Mark as in development

Assign yourself and add the `in progress` label before writing any code:

```bash
ISSUE=<number>

# create the label if it doesn't exist yet
gh label create "in progress" --color "fbca04" --description "Currently being worked on" \
  --repo AndreyShyliuk/ElShop 2>/dev/null || true

gh issue edit $ISSUE --repo AndreyShyliuk/ElShop --add-label "in progress"
gh issue comment $ISSUE --repo AndreyShyliuk/ElShop \
  --body "Starting development. Branch: \`$(echo "[XX]-short-description")\`"
```

## 3. Implement the feature

Follow **`.claude/skills/feature-dev.md`** in full:

1. Create branch: `git checkout -b <ISSUE_NUMBER>-short-description`
2. Implement using clean architecture rules
3. Write unit/integration tests as required
4. Commit with ticket reference: `[#<ISSUE_NUMBER>] Description`
5. Run tests — all must pass before pushing
6. Push branch and open a PR:

```bash
gh pr create \
  --repo AndreyShyliuk/ElShop \
  --title "[#$ISSUE] <short description>" \
  --body "$(cat <<'EOF'
## Closes #<ISSUE_NUMBER>

## What this PR does
<summary>

## Architectural decisions
<any non-obvious choices>

## How to test manually
<steps>
EOF
)"
```

The PR body must contain `Closes #<ISSUE_NUMBER>` so GitHub auto-closes the issue on merge.

## 4. After PR is merged

GitHub closes the issue automatically when the PR with `Closes #N` is merged into main.

If for any reason the issue stays open, close it manually:

```bash
gh issue close $ISSUE --repo AndreyShyliuk/ElShop \
  --comment "Merged via PR #<PR_NUMBER>. Closing."
```

Also remove the `in progress` label so the issue list stays clean:

```bash
gh issue edit $ISSUE --repo AndreyShyliuk/ElShop --remove-label "in progress"
```

## Summary flow

```
open issue (no label)
  → add "in progress" label + comment with branch name
  → create branch & implement (follow feature-dev.md)
  → push + open PR with "Closes #N"
  → PR reviewed and merged
  → issue auto-closed, "in progress" label removed
```
