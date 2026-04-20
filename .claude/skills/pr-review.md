# PR Review Skill

This skill governs automated PR review for the ElShop repository. A remote Claude agent (trigger `trig_01DneXCjrTMDwuHJ7aRXZT6L`) polls for open PRs every hour and runs this workflow.

## Trigger

**Remote agent:** https://claude.ai/code/scheduled/trig_01DneXCjrTMDwuHJ7aRXZT6L  
**Schedule:** Every hour  
**Runs in:** Anthropic cloud (not local machine)

### GitHub webhook (optional — fires the agent immediately on PR open)

To get instant review instead of waiting up to an hour, set up a GitHub webhook:

1. Go to **GitHub → ElShop repo → Settings → Webhooks → Add webhook**
2. **Payload URL:** `https://api.claude.ai/v1/code/triggers/trig_01DneXCjrTMDwuHJ7aRXZT6L/run`
3. **Content type:** `application/json`
4. **Secret:** leave blank (the trigger uses its own auth)
5. **Which events:** select **Pull requests** only → check **Opened**
6. Save. GitHub will POST the PR payload to Claude on every new PR.

> Note: The hourly poll is the reliable fallback — the webhook fires Claude immediately but the agent outcome is the same either way.

## Review checklist

When reviewing a PR diff, check for:

**Major issues (block merge, must fix):**
- Business logic in controllers (`*Controller.cs`) or Angular components — move to Application/service layer
- New feature with zero tests
- Security issues: SQL injection, XSS, exposed credentials, missing input validation at system boundaries
- Broken or missing dependency injection wiring

**Minor issues (fix before merging):**
- Style or naming inconsistencies
- Redundant comments
- Missing `readonly` on injected fields
- Unused imports
- Deprecated APIs where a drop-in replacement exists

## Fix workflow

Fix both major and minor issues before merging:

```bash
gh pr checkout <NUMBER>
# fix the issues
git commit -m "[PR Review] Fix: <description>"
git push
```

## Test commands

```bash
# Frontend (from repo root)
cd elshop.client && npm test -- --run

# Backend (once a test project exists)
dotnet test ElShop.Server
```

Both must exit 0 before merging.

## Merge

If tests pass and no major issues remain:

```bash
gh pr merge <NUMBER> --squash --delete-branch --repo AndreyShyliuk/ElShop
```

The `--delete-branch` flag removes the remote branch automatically. If using the GitHub API directly, delete the branch separately:

```bash
curl -s -X DELETE "https://api.github.com/repos/AndreyShyliuk/ElShop/git/refs/heads/<BRANCH_NAME>" \
  -H "Authorization: token $GH_TOKEN"
```

## If tests fail or blockers remain

Leave a PR comment and add the `auto-reviewed` label so the agent does not re-process:

```bash
gh pr comment <NUMBER> --body "## PR Review\n\n**Status: blocked**\n\n<explain what failed>"
gh pr edit <NUMBER> --add-label auto-reviewed
```

The `auto-reviewed` label marks the PR as processed. Remove it manually if you want the agent to re-check after fixes.
